using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
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

            var endpoint = Utils.CreateGetLoginTokenEndpoint();
            var message = new HttpRequestMessage(HttpMethod.Post, new Uri(Constants.Gateway + "/" + endpoint));
            Utils.PrepareHeaders(message, _apiKey, _apiSecret);
            message.Content = new FormUrlEncodedContent(Utils.ConstructPayloadForGetToken(token, sessionData));
            var result = await _httpClient.SendAsync(message);

            var resp = await Utils.BuildUGetTokenResponse(result);

            return resp;
        }

        public async void SetTokenData(string token, SessionData sessionData)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(sessionData.SessionId)) throw new Exception("Token or Session ID is missing!");

            var endpoint = Utils.CreateSetTokenDataEndpoint();
            var message = new HttpRequestMessage(HttpMethod.Post, new Uri(Constants.Gateway + "/" + endpoint));
            Utils.PrepareHeaders(message, _apiKey, _apiSecret);
            message.Content = new FormUrlEncodedContent(Utils.ConstructPayloadForSetToken(token, sessionData));
            await _httpClient.SendAsync(message);
        }

        public bool VerifySign(string signature, string path, Dictionary<string, string> data)
        {
            if (string.IsNullOrEmpty(signature)) return false;

            var signed = new Uri(path).PathAndQuery;
            var keysList = data.Keys.ToList();
            keysList.Sort();
            var sorted = keysList.ToDictionary(element => element, element => data[element]);

            signed = sorted.Aggregate(signed, (current, element) => current + (element.Key + element.Value));

            var sha = new HMACSHA256(Encoding.UTF8.GetBytes(_apiSecret));
            var hashedSign = sha.ComputeHash(Encoding.UTF8.GetBytes(signed));

            return signature == Encoding.UTF8.GetString(hashedSign);
        }
    }
}
