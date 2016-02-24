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

            return await Utils.BuildUResponse(result);
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

        public bool VerifyLink(string key, string signature, string deviceSecret)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(signature)) return false;
            if (string.IsNullOrEmpty(deviceSecret))
            {
                Console.WriteLine("UNLOQ.verifyLink: provided deviceSecret is not a string or empty.");
                return false;
            }

            var sha = new HMACSHA256(Encoding.UTF8.GetBytes(deviceSecret));
            var signedKey = sha.ComputeHash(Encoding.UTF8.GetBytes(key));

            return signature == Encoding.UTF8.GetString(signedKey);
        }

        public async Task<IUResponse> UpdateHooks(string loginPath, string logoutPath)
        {
            if (string.IsNullOrEmpty(loginPath) || string.IsNullOrEmpty(logoutPath)) throw new Exception("One or both of the parameters are missing!");

            var endpoint = Utils.CreateUpdateHooksEndPoint();
            var message = new HttpRequestMessage(HttpMethod.Post, new Uri(Constants.Gateway + "/" + endpoint));
            Utils.PrepareHeaders(message, _apiKey, _apiSecret);
            message.Content = new FormUrlEncodedContent(Utils.ConstructPayloadForUpdateHooks(loginPath, logoutPath));
            var result = await _httpClient.SendAsync(message);

            return await Utils.BuildUResponse(result);
        }

        public async Task<IUResponse> UpdateAppLinking(string linkPath, string unlinkPath, bool disable = false)
        {
            if (string.IsNullOrEmpty(linkPath) || string.IsNullOrEmpty(unlinkPath)) throw new Exception("linkPath or unlinkPath is missing!");

            var endpoint = Utils.CreateUpdateAppLinkingEndPoint();
            var message = new HttpRequestMessage(HttpMethod.Post, new Uri(Constants.Gateway + "/" + endpoint));
            Utils.PrepareHeaders(message, _apiKey, _apiSecret);
            message.Content = new FormUrlEncodedContent(Utils.ConstructPayloadForUpdateAppLinking(linkPath, unlinkPath, disable));
            var result = await _httpClient.SendAsync(message);

            return await Utils.BuildUResponse(result);
        }
    }
}
