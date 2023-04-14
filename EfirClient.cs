using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using Efir.DataHub.Models.Models.Account;
using Efir.DataHub.Models.Models.Info;
using Efir.DataHub.Models.Requests.V2.Account;
using Efir.DataHub.Models.Requests.V2.Info;

namespace RuDataAPI
{
    /// <summary>
    ///     Provides API to connect to EFIR.DataHub server to get market financial data.
    /// </summary>
    public class EfirClient : IDisposable
    {
        private readonly EfirCredentials _credentials;
        private readonly HttpClient _httpClient;
        private string? _token = default;
        private const string MEDIATYPE = "application/json";

        public EfirClient(EfirCredentials credentials) : base()
        {
            _credentials = credentials;
            _httpClient = new HttpClient();
        }

        public EfirCredentials Credentials => _credentials;

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



        public async Task<FintoolReferenceDataFields[]> GetSecurityData(string isin)
        {
            var query = new FintoolReferenceDataRequest
            {
                id = isin,
                fields = new string[] { "NickName", "FinToolType", "FaceValue", "FaceFTName", "CouponType", "CouponTypeName_NRD", "issuername_nrd", "faceftname",
                                        "FloatRateName", "EndMtyDate", "Offer_Date", "Status", "SumMarketVal", "IssuerSector", "fintoolid", "isincode"      }

            };
            string url = $"{_credentials.Url}/info/fintoolReferenceData";
            return await PostEfirRequestAsync<FintoolReferenceDataRequest, FintoolReferenceDataFields[]>(query, url);
        }



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
        }


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
