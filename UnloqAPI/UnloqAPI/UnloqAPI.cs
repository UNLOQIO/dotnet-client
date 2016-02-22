using System;
using System.IO;
using System.Net.Http;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UnloqAPI
{
    public class UnloqApi
    {
        private readonly string _apiKey;
        private readonly string _apiSecret;
        private readonly HttpClient _httpClient;

        public UnloqApi(string apiKey, string apiSecret)
        {
            _apiKey = apiKey;
            _apiSecret = apiSecret;

            _httpClient = new HttpClient {BaseAddress = new Uri(Constants.Gateway)};
        }

        public async Task<UResponse> AuthenticateUser(UnloqCredentials credentials, UnloqOpts options)
        {
            if (string.IsNullOrEmpty(credentials.Email)) throw new Exception("Invalid credentials!");

            var endpoint = Utils.CreateAuthorizeEndPoint();
            var message = new HttpRequestMessage(HttpMethod.Post, new Uri(Constants.Gateway + "/" + endpoint));
            Utils.PrepareHeaders(message, _apiKey, _apiSecret);
            message.Content = new FormUrlEncodedContent(Utils.ConstructPayload(credentials.Email, options.Method, credentials.Token, options.Ip));
            var result = await _httpClient.SendAsync(message);

            return await Utils.BuildUResponse(result);
        }
    }
}
