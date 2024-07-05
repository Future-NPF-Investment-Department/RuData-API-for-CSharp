#pragma warning disable IDE1006 // Naming Styles

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using Efir.DataHub.Models.Models;
using Efir.DataHub.Models.Models.Account;
using Efir.DataHub.Models.Models.Info;
using Efir.DataHub.Models.Models.Bond;
using Efir.DataHub.Models.Models.Moex;
using Efir.DataHub.Models.Models.RuData;
using Efir.DataHub.Models.Models.Rating;
using Efir.DataHub.Models.Models.Archive;
using Efir.DataHub.Models.Models.Sppi;

using Efir.DataHub.Models.Requests.V2;
using Efir.DataHub.Models.Requests.V2.Account;
using Efir.DataHub.Models.Requests.V2.Info;
using Efir.DataHub.Models.Requests.V2.Bond;
using Efir.DataHub.Models.Requests.V2.Moex;
using Efir.DataHub.Models.Requests.V2.RuData;
using Efir.DataHub.Models.Requests.V2.Rating;
using Efir.DataHub.Models.Requests.V2.Archive;
using Efir.DataHub.Models.Requests.V2.Sppi;
using EmissionDocsRequest = Efir.DataHub.Models.Requests.V2.Info.EmissionDocsRequest;

namespace RuDataAPI
{
    /// <summary>
    ///     Provides API to communicate with EFIR.DataHub server (Interfax RuData) to get market data.
    /// </summary>
    public sealed class EfirClient : IDisposable
    {
        private readonly EfirCredentials _credentials;
        private readonly HttpClient _httpClient;
        private string? _token = default;
        private const string MEDIATYPE = "application/json";

        public EfirClient(EfirCredentials credentials)
        {
            _credentials = credentials;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue(MEDIATYPE));
        }

        /// <summary>
        ///     EFIR.DataHub server authorization credentials.
        /// </summary>
        public EfirCredentials Credentials => _credentials;

        /// <summary>
        ///     EFIR.DataHub server Authorization state.
        /// </summary>
        public bool IsLoggedIn { get; private set; }

        /// <summary>
        ///     Releases unmanaged resources.
        /// </summary>
        public void Dispose()        
            => _httpClient.Dispose();


        /// <summary>
        ///     Obtains EfirCredentials from json file. If failed returns empty EfirCredentials.
        /// </summary>
        /// <param name="credentialsFilePath">Path to file that contains credentials to login Efir server.</param>
        public static EfirCredentials GetCredentialsFromFile(string credentialsFilePath)
        {
            if (File.Exists(credentialsFilePath) is false)
                throw new Exception($"File {credentialsFilePath} does not exist.");

            using FileStream fs = new(credentialsFilePath, FileMode.Open);
            return JsonSerializer.Deserialize<EfirCredentials>(fs) ?? EfirCredentials.Empty;
        }


        /// <summary>
        ///     Sends POST reuest to login to EFIR server using preloaded credentials.
        /// </summary>
        /// <remarks>
        ///     To load credentials from file use static <see cref="GetCredentialsFromFile"/> method.
        ///     <para>
        ///         For more details about usage see <see href="https://docs.efir-net.ru/dh2/#/Account/Login">
        ///         https://docs.efir-net.ru/dh2/#/Account/Login
        ///         </see>.
        ///     </para>
        /// </remarks>
        public async Task LoginAsync()
        {
            var query = new LoginRequest
            {
                login = _credentials.Login,
                password = _credentials.Password
            };
            string url = $"{_credentials.Url}/account/login";
            LoginResponse res = await PostEfirRequestAsync<LoginRequest, LoginResponse>(query, url);
            _token = res.Token;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            IsLoggedIn = true;
        }


        /// <summary>
        ///     Sends POST request to EFIR Server to get list of Russian holidays for specified year. If year is null current year is used.
        /// </summary>
        /// <remarks>
        ///     For more details about usage see <see href="https://docs.efir-net.ru/dh2/#/Info/Holidays?id=post-holidays">
        ///         https://docs.efir-net.ru/dh2/#/Info/Holidays?id=post-holidays
        ///     </see>.
        /// </remarks>
        /// <param name="year">Year for which list of holidays is returned.</param>
        /// <returns>Array of <see cref="HolidaysFields"/>.</returns>
        public async Task<HolidaysFields[]> GetHolidaysAsync(DateTime start, DateTime end, Calendar cdr)
        {
            var query = new HolidaysRequest
            {
                CountryId = (int)cdr,
                BeginDate = start,
                EndDate = end,
                CalendarTypeId = CalendarTypes.Country
            };
            var url = $"{_credentials.Url}/Info/Holidays";
            return await PostEfirRequestAsync<HolidaysRequest, HolidaysFields[]>(query, url);
        }


        /// <summary>
        ///     Sends POST request to EFIR Server to get all possible enumerations.
        /// </summary>
        /// <remarks>
        ///     For more details about usage see <see href="https://docs.efir-net.ru/dh2/#/Info/Enums">
        ///         https://docs.efir-net.ru/dh2/#/Info/Enums
        ///     </see>.
        /// </remarks>
        public async Task<EnumsFields[]> GetAllEfirEnumerations()
        {
            var query = new EnumsRequest();
            string url = $"{_credentials.Url}/Info/Enums";
            return await PostEfirRequestAsync<EnumsRequest, EnumsFields[]>(query, url);
        }


        /// <summary>
        ///     Sends POST request to EFIR Server to get all possible fields for specified enumeration.
        /// </summary>
        /// <param name="dictionaryName">Efir server method name. Two possible values: ReferenceData and Securities</param>
        /// <param name="enumName">Efir enumeration name.</param>
        /// <remarks>
        ///     For more details about usage see <see href="https://docs.efir-net.ru/dh2/#/Info/EnumValues">
        ///         https://docs.efir-net.ru/dh2/#/Info/EnumValues
        ///     </see>.
        /// </remarks>
        public async Task<EnumValuesFields[]> GetEfirEnumValues(string dictionaryName, string enumName)
        {
            var query = new EnumValuesRequest
            {
                dictionaryName = dictionaryName,
                fieldName = enumName
            };
            string url = $"{_credentials.Url}/Info/EnumValues";
            return await PostEfirRequestAsync<EnumValuesRequest, EnumValuesFields[]>(query, url);
        }


        /// <summary>
        ///     Sends POST request to EFIR Server to get static data for chosen security.
        /// </summary>
        /// <param name="isin">Security ISIN.</param> 
        /// <remarks>
        ///     For more details about usage see <see href="https://docs.efir-net.ru/dh2/#/Info/FintoolReferenceData">
        ///         https://docs.efir-net.ru/dh2/#/Info/FintoolReferenceData
        ///     </see>.
        /// </remarks>
        /// <returns>
        ///     Array of <see cref="FintoolReferenceDataFields"/>.
        /// </returns>
        public async Task<FintoolReferenceDataFields> GetFinToolRefDataAsync(string isin, RefDataCols columns = RefDataCols.ALL)
        {
            var query = new FintoolReferenceDataRequest
            {
                id = isin,
                fields = GetColumnNames(columns).ToArray()
            };
            string url = $"{_credentials.Url}/Info/fintoolReferenceData";
            var list =  await PostEfirRequestAsync<FintoolReferenceDataRequest, List<FintoolReferenceDataFields>>(query, url);
            if (list.Count is 0) throw new Exception($"EFIR: Security with ISIN code {isin} not found.");            
            return list[0];
        }


        /// <summary>
        ///     Sends POST request to EFIR Server to find all securities that match specified criteria.
        /// </summary>
        /// <param name="filter">Filtration string. SQL WHERE clause syntax used.</param> 
        /// <remarks>
        ///     For more details about usage see 
        ///     <see href="https://docs.efir-net.ru/dh2/#/Info/FintoolReferenceData">
        ///         https://docs.efir-net.ru/dh2/#/Info/FintoolReferenceData</see>.
        ///     <para>Please use SQL WHERE clause syntax to specify filtration string.</para>
        /// </remarks>
        /// <returns>
        ///     Array of <see cref="FintoolReferenceDataFields"/>.
        /// </returns>
        public async Task<FintoolReferenceDataFields[]> FindSecuritiesAsync(string filter)
        {
            var query = new FintoolReferenceDataRequest
            {
                pager = new Efir.DataHub.Models.Requests.V2.Info.Pager() { Page = 1, Size = 1000 },
                filter = filter,
                fields = GetColumnNames(RefDataCols.ALL).ToArray()
            };
            string url = $"{_credentials.Url}/Info/fintoolReferenceData";
            return await PostEfirPagedRequestAsync<FintoolReferenceDataFields>(query, url, 1000);
        }


        /// <summary>
        ///     Sends POST request to EFIR Server to get payments (incl. coupon, notional, etc.) schedule for specified bond.
        /// </summary>
        /// <param name="secIds">Secuirty ID in Efir database.</param>
        /// <remarks>
        ///     For more details about usage see <see href="https://docs.efir-net.ru/dh2/#/Info/CalendarV2">
        ///         https://docs.efir-net.ru/dh2/#/Info/CalendarV2
        ///     </see>.
        /// </remarks>
        /// <returns></returns>
        public async Task<TimeTableV2Fields[]> GetEventsCalendarAsync(params string[] isins)
        {
            if (isins.Length == 0) return Array.Empty<TimeTableV2Fields>();

            // max 100 ISIN codes are allowed in 1 query
            // split set of inn codes to chunks of size 100.
            var chunks = isins.Chunk(100);
            var requests = new List<Task<TimeTableV2Fields[]>>();

            // constructing request elements
            string oper = "TYPEOPERATION NOT IN ('A', 'L', 'V', 'R', 'N', 'M', 'E', 'J', 'T')";
            string url = $"{_credentials.Url}/Info/CalendarV2";

            // create and run post requests for each chunk.
            foreach (string[] chunk in chunks)
            {
                string isin = $"ISINCODE IN ('{string.Join("', '", chunk)}')";
                string filter = string.Join(" AND ", oper, isin);

                var query = new CalendarV2Request
                {
                    pageNum = 1,
                    pageSize = 1000,
                    Filter = filter
                };

                var task = PostEfirPagedRequestAsync<CalendarV2Request, TimeTableV2Fields>(query, url, 1000);
                requests.Add(task);
            }

            // wait all requests to be responded by server
            var responses = await Task.WhenAll(requests);

            // constructing response
            var response = Enumerable.Empty<TimeTableV2Fields>();
            foreach (var r in responses)
                response = response.Concat(r);

            return response.ToArray();
        }


        /// <summary>
        ///     Sends POST request to EFIR Server to get ratings history for list of issuers.
        /// </summary>
        /// <remarks>
        ///     For more details about usage see <see href="https://docs.efir-net.ru/dh2/#/Rating/RatingsHistory">
        ///         https://docs.efir-net.ru/dh2/#/Rating/RatingsHistory
        ///     </see>.
        /// </remarks>
        /// <param name="inns">List of issuer INN codes.</param>
        /// <returns>Array of <see cref="RatingsHistoryFields"/>.</returns>
        public async Task<RatingsHistoryFields[]> GetRatingHistoryAsync(DateTime start, DateTime end, string filter)
        {
            var query = new RatingsHistoryRequest
            {
                sort = 1,
                filter = filter,
                dateFrom = start,
                dateTo = end,
            };

            string url = $"{_credentials.Url}/Rating/RatingsHistory";
            var res = await PostEfirPagedRequestAsync<RatingsHistoryRequest, RatingsHistoryFields>(query, url, 300);
            return res.ToArray();
        }

        /// <summary>
        ///     Sends POST request to EFIR Server to get indexes history data.
        /// </summary>
        /// <remarks>
        ///     For more details about usage see <see href="https://docs.efir-net.ru/dh2/#/Moex/History">
        ///         https://docs.efir-net.ru/dh2/#/Moex/History
        ///     </see>.
        /// </remarks>
        /// <returns>
        ///     Array of <see cref="HistoryStockIndexFields"/>.
        /// </returns>
        public async Task<HistoryStockIndexFields[]> GetMoexIndexHistoryAsync(DateTime start, DateTime end, params string[] tickers)        
            => await GetMoexHistoryAsync<HistoryStockIndexFields>(start, end, "index", tickers);


        /// <summary>
        ///     Sends POST request to EFIR Server to get bonds history data.
        /// </summary>
        /// <remarks>
        ///     For more details about usage see <see href="https://docs.efir-net.ru/dh2/#/Moex/History">
        ///         https://docs.efir-net.ru/dh2/#/Moex/History
        ///     </see>.
        /// </remarks>
        /// <returns>
        ///     Array of <see cref="HistoryStockBondsFields"/>.
        /// </returns>
        public async Task<HistoryStockBondsFields[]> GetMoexBondHistoryAsync(DateTime start, DateTime end, params string[] tickers)        
            => await GetMoexHistoryAsync<HistoryStockBondsFields>(start, end, "bonds", tickers);


        /// <summary>
        ///     Sends POST request to EFIR Server to get shares history data.
        /// </summary>
        /// <remarks>
        ///     For more details about usage see <see href="https://docs.efir-net.ru/dh2/#/Moex/History">
        ///         https://docs.efir-net.ru/dh2/#/Moex/History
        ///     </see>.
        /// </remarks>
        /// <returns>
        ///     Array of <see cref="HistoryStockBondsFields"/>.
        /// </returns>
        public async Task<HistoryStockSharesFields[]> GetMoexStockHistoryAsync(DateTime start, DateTime end, params string[] tickers)
            => await GetMoexHistoryAsync<HistoryStockSharesFields>(start, end, "shares", tickers);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="isins"></param>
        /// <returns></returns>
        public async Task<EndOfDayOnExchangeFields[]> GetTradeHistoryAsync(DateTime start, DateTime end, params string[] isins)
        {
            // max 20 ISIN codes are allowed in 1 query
            // split set of inn codes to chunks of size 100.
            var chunks = isins.Chunk(20);
            var requests = new List<Task<EndOfDayOnExchangeFields[]>>();
            string url = $"{_credentials.Url}/Archive/EndOfDayOnExchanges";

            // create and run post requests for each chunk.
            foreach (string[] chunk in chunks)
            {
                var query = new EndOfDayOnExchangesRequest
                {
                    Codes = chunk,
                    DateFrom = start,
                    DateTo = end,
                    Fields = new[] { "last", "open", "high", "low", "facevalue", "val_acc", "lclose", "last_yield",
                                     "mcap", "isin", "time", "exch", "deal_acc", "counter", "boardid", "accruedint",
                                     "vol_acc", "deal_acc" },
                    UseDefaultTradeSite = true,
                };

                var task = PostEfirPagedRequestAsync2<EndOfDayOnExchangesRequest, EndOfDayOnExchangeFields>(query, url, 100);
                requests.Add(task);
            }

            // wait all requests to respond by server
            var responses = await Task.WhenAll(requests);

            // constructing response
            var response = Enumerable.Empty<EndOfDayOnExchangeFields>();
            foreach (var r in responses)
                response = response.Concat(r);

            return response.ToArray();
        }


        /// <summary> 
        ///     Sends POST request to EFIR Server to get links to emission docs for chosen security.
        /// </summary>
        /// <param name="isin">Security ISIN.</param>
        /// <returns>
        ///     Instance of <see cref="EmissionDocsResponse"/>.
        /// </returns>
        /// <remarks>
        ///     For more details about usage see <see href="https://docs.efir-net.ru/dh2/#/Info/EmissionDocs">
        ///         https://docs.efir-net.ru/dh2/#/Info/EmissionDocs
        ///     </see>.
        /// </remarks>
        public async Task<EmissionDocsResponse> GetEmissionDocsAsync(params string[] isins)
        {
            var query = new EmissionDocsRequest
            {
                ids = isins,
            };
            string url = $"{_credentials.Url}/Info/EmissionDocs";
            return await PostEfirRequestAsync<EmissionDocsRequest, EmissionDocsResponse>(query, url);
        }

        /// <summary>
        ///     Sends POST request to EFIR Server to get SPPI testing results for specified securities as of specified date
        /// </summary>
        /// <param name="date"></param>
        /// <param name="fintoolids"></param>
        /// <returns>
        ///     Array of <see cref="TestingFields"/>.
        /// </returns>
        /// <remarks>
        ///     For more details about usage see <see href="https://docs.efir-net.ru/dh2/#/Sppi/Testing">
        ///         https://docs.efir-net.ru/dh2/#/Sppi/Testing
        ///     </see>.
        /// </remarks>
        public async Task<TestingV2Fields[]> GetSppiTestingResults(DateTime date, params string[] isins)
        {
            var query = new TestingV2Request
            {
                Codes = isins,
                CalcDate = date,                
            };
            string url = $"{_credentials.Url}/SppiV2/Testing";
            return await PostEfirRequestAsync<TestingV2Request, TestingV2Fields[]>(query, url);
        }

        /// <summary> 
        ///     Sends POST request to EFIR Server to get parameters of MOEX yield curve (GCurve) for specified date.
        /// </summary>
        /// <param name="date">Date of yield curve.</param>
        /// <remarks>
        ///     For more details about usage see <see href="https://docs.efir-net.ru/dh2/#/Bond/g-curve-ofz?id=post-g-curve-ofz">
        ///         https://docs.efir-net.ru/dh2/#/Bond/g-curve-ofz?id=post-g-curve-ofz
        ///     </see>.
        ///     <para> For more details about GCurve construction methodology see <see href="https://www.moex.com/s2532">
        ///             MOEX GCurve reference page</see>. 
        ///     </para>    
        /// </remarks>
        /// <returns>
        ///     Instance of <see cref="GCurveOFZResponse"/>.
        /// </returns>
        public async Task<GCurveOFZResponse> GetGcurveParametersAsync(DateTime date)
        {
            var query = new GCurveOFZRequest
            {
               Date = date,
            };

            string url = $"{_credentials.Url}/Bond/g-curve-ofz";
            return await PostEfirRequestAsync<GCurveOFZRequest, GCurveOFZResponse>(query, url);
        }

        /// <summary> 
        ///     Sends POST request to EFIR Server to get parameters of floating bonds.
        /// </summary>
        /// <param name="ids">Security identificators.</param>
        /// <returns>
        ///     Array of <see cref="FloaterDataFields"/>.
        /// </returns>
        /// <remarks>
        ///     For more details about usage see <see href="https://docs.efir-net.ru/dh2/#/Bond/FloaterData">
        ///         https://docs.efir-net.ru/dh2/#/Bond/FloaterData
        ///     </see>. 
        /// </remarks>
        public async Task<FloaterDataFields[]> GetFloaterDataAsync(params long[] ids)
        {
            var query = new FloaterDataRequest
            {
                FintoolIds=ids,
                ShowFuturePeriods = true
            };
            string url = $"{_credentials.Url}/Bond/FloaterData";
            return await PostEfirRequestAsync<FloaterDataRequest, FloaterDataFields[]>(query, url);
        }

        /// <summary>
        ///     Returns history data for instrument tarded on MOEX.
        /// </summary>
        private async Task<TFields[]> GetMoexHistoryAsync<TFields>(DateTime start, DateTime end, string mkt, params string[] tickers)
            where TFields : HistoryBaseFields, new()
        {
            var query = new Efir.DataHub.Models.Requests.V2.Moex.HistoryRequest
            {
                engine = "stock",
                market = mkt,
                instruments = tickers,
                dateFrom = start,
                dateTo = end
            };
            string url = $"{_credentials.Url}/Moex/History";
            return await PostEfirPagedRequestAsync2<Efir.DataHub.Models.Requests.V2.Moex.HistoryRequest, TFields>(query, url, 1000);
        }


        /// <summary>
        ///     Sends POST request to specified url of EFIR server.
        /// </summary>
        private async Task<TResponse> PostEfirRequestAsync<TRequest, TResponse>(TRequest request, string url)
        {
            string jsonRequest = JsonSerializer.Serialize(request);
            HttpContent content = new StringContent(jsonRequest, Encoding.UTF8, MEDIATYPE);
            HttpResponseMessage response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode is false)            
                throw new HttpRequestException(await response.Content.ReadAsStringAsync());            
                
            return await response.Content.ReadAsAsync<TResponse>();                        
        }


        /// <summary>
        ///     Sends paged POST request to specified url of EFIR server.
        /// </summary>
        private async Task<TResponse[]> PostEfirPagedRequestAsync<TRequest, TResponse>(TRequest request, string url, int pageSize)
            where TRequest : IPagedRequest
            where TResponse : IPagingBase
        {
            TResponse[] response = await PostEfirRequestAsync<TRequest, TResponse[]>(request, url);
            if (response.Length > 0 && response[0].counter > pageSize)
            {
                int npages = response[0].counter / pageSize + 1;
                var requests = new Task<TResponse[]>[npages - 1];
                for (int i = 2; i <= npages; i++)
                {
                    request.pageNum = i;
                    requests[i - 2] = PostEfirRequestAsync<TRequest, TResponse[]>(request, url);
                }
                var responses = await Task.WhenAll(requests.Where(t => t is not null));
                foreach (var resp in responses)
                    response = response.Concat(resp).ToArray();
            }
            return response;
        }


        private async Task<TResponse[]> PostEfirPagedRequestAsync<TResponse>(IFXFintoolRefDataRequest request, string url, int pageSize)
            where TResponse : IPagingBase
        {
            TResponse[] response = await PostEfirRequestAsync<IFXFintoolRefDataRequest, TResponse[]>(request, url);
            if (response.Length > 0 && response[0].counter > pageSize)
            {
                int npages = response[0].counter / pageSize + 1;
                var requests = new Task<TResponse[]>[npages - 1];
                for (int i = 2; i <= npages; i++)
                {
                    request.pager.Page = i;
                    requests[i - 2] = PostEfirRequestAsync<IFXFintoolRefDataRequest, TResponse[]>(request, url);
                }
                var responses = await Task.WhenAll(requests.Where(t => t is not null));
                foreach (var resp in responses)
                    response = response.Concat(resp).ToArray();
            }
            return response;
        }


        /// <summary>
        ///     Sends paged POST request to specified url of EFIR server. 
        /// </summary>
        /// <remarks>
        ///     This method uses <see cref="ICounterBase"/> constraint for <typeparamref name="TResponse"/> instead of 
        ///     <see cref="IPagingBase"/> as in <see cref="PostEfirPagedRequestAsync{TRequest,TResponse}"/> method.
        /// </remarks>
        private async Task<TResponse[]> PostEfirPagedRequestAsync2<TRequest, TResponse>(TRequest request, string url, int pageSize)
            where TRequest : IPagedRequest
            where TResponse : ICounterBase
        {
            TResponse[] response = await PostEfirRequestAsync<TRequest, TResponse[]>(request, url);
            if (response.Length > 0 && response[0].counter > pageSize)
            {
                int npages = response[0].counter!.Value / pageSize + 1;
                var requests = new Task<TResponse[]>[npages - 1];
                for (int i = 2; i <= npages; i++)
                {
                    request.pageNum = i;
                    requests[i - 2] = PostEfirRequestAsync<TRequest, TResponse[]>(request, url);
                }
                var responses = await Task.WhenAll(requests.Where(t => t is not null));
                foreach (var resp in responses)
                    response = response.Concat(resp).ToArray();
            }
            return response;
        }


        /// <summary>
        ///     Transforms <see cref="RefDataCols"/> bitmask to collection of strings.
        /// </summary>
        private static IEnumerable<string> GetColumnNames(RefDataCols refDataCols)
        {
            var fields = Enum.GetNames<RefDataCols>();
            foreach (var f in fields) 
                if (refDataCols.HasFlag(Enum.Parse<RefDataCols>(f))) 
                    yield return f.ToString();
        }
    }
}
