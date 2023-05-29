using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using Efir.DataHub.Models.Models.Account;
using Efir.DataHub.Models.Models.Info;
using Efir.DataHub.Models.Models.Moex;
using Efir.DataHub.Models.Models.RuData;

using Efir.DataHub.Models.Requests.V2.Account;
using Efir.DataHub.Models.Requests.V2.Info;
using Efir.DataHub.Models.Requests.V2.Moex;
using Efir.DataHub.Models.Requests.V2.RuData;

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
        /// <param name="credentialsFilePath">
        ///     Path to file that contains credentials to login Efir server.
        /// </param>
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
        ///     To load credentials from file use <see cref="GetSecurityDataAsync"/> method.
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
        ///     Sends POST request to EFIR Server to get static data for chosen security.
        /// </summary>
        /// <param name="isin">
        ///     Security ISIN.    
        /// </param> 
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
        /// <param name="isin">
        ///     Security ISIN.
        /// </param>
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
            var query = new EmissionDocsRequest
            {
                ids = new string[] { isin },
            };
            string url = $"{_credentials.Url}/Info/EmissionDocs";
            return await PostEfirRequestAsync<EmissionDocsRequest, EmissionDocsResponse>(query, url);
        }


        /// <summary> 
        ///     Sends POST request to EFIR Server to get parameters of MOEX yield curve (GCurve) for specified date.
        /// </summary>
        /// <param name="date">
        ///     Date of yield curve.
        /// </param>
        /// <returns>
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
            string mes = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<TResponse>();                        
        }

        public void Dispose()        
            => _httpClient.Dispose();
        
    }
}
