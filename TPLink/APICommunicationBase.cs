using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TPLink
{
    public sealed class APICommunicationBase
    {
        private static readonly APICommunicationBase instance = new APICommunicationBase();
        private static readonly HttpClient _client = new HttpClient()
        {
            BaseAddress = new Uri("https://wap.tplinkcloud.com"),
        };

        private static string _AuthenticationToken;

        private static DateTime? _TokenGenerateDate;

        public static string AuthenticationToken
        {
            get
            {
                var generateDateComparison = (_TokenGenerateDate ?? new DateTime()).AddHours(4).CompareTo(DateTime.UtcNow) <= 0;
                if (String.IsNullOrEmpty(_AuthenticationToken) || generateDateComparison)
                {
                    if(generateDateComparison && _TokenGenerateDate != null) {
                        Console.WriteLine("Generation of token was greater than four hours ago. New token forced.");
                    }
                    GetAuthenticationToken().GetAwaiter().GetResult();
                }

                return _AuthenticationToken;
            }
        }

        static APICommunicationBase() { }

        private APICommunicationBase() { }

        public static APICommunicationBase Instance
        {
            get
            {
                return instance;
            }
        }

        public static async Task<string> MakePostRequest(object requestBody, string requestUri = "")
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync(requestUri, requestBody);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();

            throw new IOException($"Failed to generate HTTP POST against {requestUri}");
        }

        public static async Task<string> GetAuthenticationToken()
        {
            string response = await MakePostRequest(
                new APIRequestParams<APIAuthenticationRequest>
                {
                    method = "login",
                    requestParams = BuildBaseRequestParams(),
                }
            );

            string token = (JsonConvert.DeserializeObject<APIResponse<APIAuthenticationResponse>>(response)).result.token;
            _AuthenticationToken = token;
            _TokenGenerateDate = DateTime.UtcNow;
            return token;
        }
        private static APIAuthenticationRequest BuildBaseRequestParams()
        {
            using (StreamReader reader = new StreamReader("./Settings.json"))
            {
                string rawJson = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<APIAuthenticationRequest>(rawJson);
            }
        }

        private class APIAuthenticationRequest
        {
            public string appType { get { return "Kasa_Fake"; } }

            public string cloudUserName { get; set; }

            public string cloudPassword { get; set; }

            public string terminalUUID { get; set; }
        }

        private class APIAuthenticationResponse
        {
            public string accountId { get; set; }
            public string email { get; set; }
            public DateTime regTime { get; set; }
            public string token { get; set; }
        }
    }
}