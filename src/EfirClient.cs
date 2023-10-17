using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using Efir.DataHub.Models.Models.Account;
using Efir.DataHub.Models.Models.Info;
using Efir.DataHub.Models.Models.Bond;
using Efir.DataHub.Models.Models.Moex;
using Efir.DataHub.Models.Models.RuData;
using Efir.DataHub.Models.Models.Rating;
using CommonDataFields = Efir.DataHub.Models.Models.Nsd.CommonDataFields;

using Efir.DataHub.Models.Requests.V2.Account;
using Efir.DataHub.Models.Requests.V2.Info;
using Efir.DataHub.Models.Requests.V2.Bond;
using Efir.DataHub.Models.Requests.V2.Moex;
using Efir.DataHub.Models.Requests.V2.RuData;
using Efir.DataHub.Models.Requests.V2.Rating;
using CommonDataRequest = Efir.DataHub.Models.Requests.V2.Nsd.CommonDataRequest;
using Efir.DataHub.Models.Models.Nsd;

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
        ///     To load credentials from file use static <see cref="GetSecurityDataAsync"/> method.
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
            IsLoggedIn = true;
        }


        /// <summary>
        ///     Sends POST request to EFIR Server to get all possible enumerations.
        /// </summary>
        /// <remarks>
        ///     For more details about usage see <see href="https://docs.efir-net.ru/dh2/#/Info/Enums">
        ///         https://docs.efir-net.ru/dh2/#/Info/Enums
        ///     </see>.
        /// </remarks>
        /// <returns></returns>
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
        /// <returns></returns>
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
        public async Task<FintoolReferenceDataFields[]> GetSecurityDataAsync(string isin)
        {
            var query = new FintoolReferenceDataRequest
            {
                id = isin,
                fields = new string[] { "NickName", "fullname", "FinToolType", "FaceValue", "CouponType", "CouponTypeName_NRD", "issuername_nrd", "faceftname",
                                        "FloatRateName", "EndMtyDate", "Offer_Date", "Status", "SumMarketVal", "IssuerSector", "fintoolid", "isincode",
                                        "coupontypename_nrd", "begdistdate", "enddistdate", "daysall", "firstcoupondate", "ismatched", "numcoupons" }

            };
            string url = $"{_credentials.Url}/Info/fintoolReferenceData";
            return await PostEfirRequestAsync<FintoolReferenceDataRequest, FintoolReferenceDataFields[]>(query, url);
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
                filter = filter,
                fields = new string[] { "NickName", "fullname", "FinToolType", "FaceValue", "CouponType", "CouponTypeName_NRD", "issuername_nrd", "faceftname", "issuercountry",
                                        "FloatRateName", "EndMtyDate", "Offer_Date", "Status", "SumMarketVal", "IssuerSector", "fintoolid", "isincode", "issuerinn",
                                        "coupontypename_nrd", "begdistdate", "enddistdate", "daysall", "firstcoupondate", "ismatched", "numcoupons", "borrowerinn" }

            };
            string url = $"{_credentials.Url}/Info/fintoolReferenceData";
            return await PostEfirRequestAsync<FintoolReferenceDataRequest, FintoolReferenceDataFields[]>(query, url);
        }

        public async Task<CommonDataFields[]> GetNsdSecuritiesDataAsync(string filter, int pagenum)
        {
            var query = new CommonDataRequest
            {
                filter = filter,     
                pageNum = pagenum,
                pageSize = 100
            };
            string url = $"{_credentials.Url}/Nsd/CommonData";
            return await PostEfirRequestAsync<CommonDataRequest, CommonDataFields[]>(query, url);
        }

        public async Task<NsdEmitentsFields[]> GetNsdEmitentsDataAsync(string filter)
        {
            var query = new EmitentsRequest
            {
                filter = filter,
            };
            string url = $"{_credentials.Url}/Nsd/Emitents";
            return await PostEfirRequestAsync<EmitentsRequest, NsdEmitentsFields[]>(query, url);
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
        public async Task<TimeTableV2Fields[]> GetEventsCalendarAsync(params long[] secIds)
        {
            var query = new CalendarV2Request
            {
                FintoolIds = secIds,
                Filter = "typeOperation NOT IN ('A', 'L', 'V', 'R', 'N', 'M', 'E', 'J', 'T')"
            };

            string url = $"{_credentials.Url}/Info/CalendarV2";
            return await PostEfirRequestAsync<CalendarV2Request, TimeTableV2Fields[]>(query, url);
        }

        /// <summary>
        ///     Sends POST request to EFIR Server to get security ratings.
        /// </summary>
        /// <remarks>
        ///     For more details about usage see <see href="https://docs.efir-net.ru/dh2/#/Rating/SecurityRatings">
        ///         https://docs.efir-net.ru/dh2/#/Rating/SecurityRatings
        ///     </see>.
        /// </remarks>
        /// <param name="isin">Security ISIN-code.</param>
        /// <returns>Array of <see cref="SecurityRatingsFields"/>.</returns>
        public async Task<SecurityRatingTableFields[]> GetRatingAsync(string isin)
        {
            var query = new SecurityRatingsTableRequest
            {
                ids = new string[] { isin },
            };

            string url = $"{_credentials.Url}/Rating/SecurityRatingTable";
            return await PostEfirRequestAsync<SecurityRatingsTableRequest, SecurityRatingTableFields[]>(query, url);
        }

        /// <summary>
        ///     Sends POST request to EFIR Server to get security ratings history.
        /// </summary>
        /// <remarks>
        ///     For more details about usage see <see href="https://docs.efir-net.ru/dh2/#/Rating/RatingsHistory">
        ///         https://docs.efir-net.ru/dh2/#/Rating/RatingsHistory
        ///     </see>.
        /// </remarks>
        /// <param name="isin">Security ISIN-code.</param>
        /// <param name="inn">Security issuer inn code.</param>
        /// <returns>Array of <see cref="RatingsHistoryFields"/>.</returns>
        public async Task<RatingsHistoryFields[]> GetRatingHistoryAsync(string innOrIsin)
        {
            string iscr = "is_credit_rating = 1";
            string ra = "rating_agency in ('Moody''s', 'Standard & Poor''s', 'Fitch Ratings', 'АКРА', 'Эксперт РА', 'НКР', 'НРА')";
            string term = "rating_term = 'Долгосрочный'";
            string filter = $"(inn = '{innOrIsin}' OR isin = '{innOrIsin}')";

            var query = new RatingsHistoryRequest
            {
                sort = 1,
                filter = string.Join(" AND ", iscr, ra, term, filter)
            };

            string url = $"{_credentials.Url}/Rating/RatingsHistory";
            return await PostEfirRequestAsync<RatingsHistoryRequest, RatingsHistoryFields[]>(query, url);
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
        {
            var query = new HistoryRequest
            {
                engine = "stock",
                market = "index",
                instruments = tickers,
                dateFrom = start,
                dateTo = end
            };
            string url = $"{_credentials.Url}/Moex/History";
            return await PostEfirRequestAsync<HistoryRequest, HistoryStockIndexFields[]>(query, url);
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
        public async Task<EmissionDocsResponse> GetEmissionDocsAsync(string isin)
        {
            var query = new Efir.DataHub.Models.Requests.V2.Info.EmissionDocsRequest
            {
                ids = new string[] { isin },
            };
            string url = $"{_credentials.Url}/Info/EmissionDocs";
            return await PostEfirRequestAsync<Efir.DataHub.Models.Requests.V2.Info.EmissionDocsRequest, EmissionDocsResponse>(query, url);
        }


        /// <summary> 
        ///     Sends POST request to EFIR Server to get parameters of MOEX yield curve (GCurve) for specified date.
        /// </summary>
        /// <param name="date">Date of yield curve.<returns>
        ///     Instance of <see cref="GCurveOFZResponse"/>.
        /// </returns>
        /// <remarks>
        ///     For more details about usage see <see href="https://docs.efir-net.ru/dh2/#/Bond/g-curve-ofz?id=post-g-curve-ofz">
        ///         https://docs.efir-net.ru/dh2/#/Bond/g-curve-ofz?id=post-g-curve-ofz
        ///     </see>.
        ///     <para> For more details about GCurve construction methodology see <see href="https://www.moex.com/s2532">
        ///             MOEX GCurve reference page</see>. 
        ///     </para>    
        /// </remarks>
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
        ///     Private method to send POST request to specified url of EFIR server.
        /// </summary>
        private async Task<TResponse> PostEfirRequestAsync<TRequest, TResponse>(TRequest request, string url)
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MEDIATYPE));
            _httpClient.DefaultRequestHeaders.Authorization = string.IsNullOrEmpty(_token) is false
                ? new AuthenticationHeaderValue("Bearer", _token)
                : null;

            string jsonRequest = JsonSerializer.Serialize(request);
            HttpResponseMessage response = await _httpClient.PostAsync(url, new StringContent(jsonRequest, Encoding.UTF8, MEDIATYPE));
            if (response.IsSuccessStatusCode is false)            
                throw new HttpRequestException(await response.Content.ReadAsStringAsync());            
                
            return await response.Content.ReadAsAsync<TResponse>();                        
        }

        public void Dispose()        
            => _httpClient.Dispose();
        
    }
}
