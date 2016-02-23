using System;
using System.Net.Http;
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

        public async Task<IUResponse> AuthenticateUser(UnloqCredentials credentials, UnloqOpts options)
        {
            if (string.IsNullOrEmpty(credentials.Email)) throw new Exception("Email is NOT optional!");

            var endpoint = Utils.CreateAuthorizeEndPoint();
            var message = new HttpRequestMessage(HttpMethod.Post, new Uri(Constants.Gateway + "/" + endpoint));
            Utils.PrepareHeaders(message, _apiKey, _apiSecret);
            message.Content = new FormUrlEncodedContent(Utils.ConstructPayloadForAuthorization(credentials.Email, options.Method, options.Ip, credentials.Token));
            var result = await _httpClient.SendAsync(message);

            var sterge = await Utils.BuildUAuthResponse(result);

            return sterge;
        }

        public async Task<IUResponse> GetLoginToken(string token, SessionData sessionData = null)
        {
            if (string.IsNullOrEmpty(token)) throw new Exception("Token is NOT optional!");

            var endpoint = Utils.CreateGetLoginEndpoint();
            var message = new HttpRequestMessage(HttpMethod.Post, new Uri(Constants.Gateway + "/" + endpoint));
            Utils.PrepareHeaders(message, _apiKey, _apiSecret);
            message.Content = new FormUrlEncodedContent(Utils.ConstructPayloadForGetToken(token, sessionData));
            var result = await _httpClient.SendAsync(message);

            var resp = await Utils.BuildUGetTokenResponse(result);

            return resp;
        }
    }
}
