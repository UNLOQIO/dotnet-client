using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace UnloqAPI
{
    public class Utils
    {
        public static string CreateAuthorizeEndPoint()
        {
            return "v" + Constants.Version + "/authenticate";
        }

        public static void PrepareHeaders(HttpRequestMessage message, string key, string secret)
        {
            message.Headers.Add("X-Api-Key", key);
            message.Headers.Add("X-Api-Secret", secret);
        }

        public static Dictionary<string, string> ConstructPayloadForAuthorization(string email, UnloqMethod method, string ip = "",
            string token = "")
        {
            return new Dictionary<string, string>
            {
               { "email", email },
               { "method", method.ToString() },
               { "ip", ip },
               { "token", token }
            };
        }

        public static Dictionary<string, string> ConstructPayloadForGetToken(string token, SessionData sessionData)
        {
            return new Dictionary<string, string>
            {
                { "token", token },
                { "sid", sessionData == null ? "" : sessionData.SessionId },
                { "duration", sessionData == null ? "" : sessionData.Duration.ToString() }
            };
        }

        public static Dictionary<string, string> ConstructPayloadForSetToken(string token, SessionData sessionData)
        {
            return new Dictionary<string, string>
            {
                { "token", token },
                { "sid", sessionData.SessionId },
                { "duration", sessionData == null ? "" : sessionData.Duration.ToString() }
            };
        }

        public static async Task<UAuthResponse> BuildUAuthResponse(HttpResponseMessage response)
        {
            return await response.Content.ReadAsAsync<UAuthResponse>();
        }

        public static async Task<UGetTokenResponse> BuildUGetTokenResponse(HttpResponseMessage response)
        {
            return await response.Content.ReadAsAsync<UGetTokenResponse>();
        }

        public static string CreateGetLoginTokenEndpoint()
        {
            return "v" + Constants.Version + "/token";
        }

        public static string CreateSetTokenDataEndpoint()
        {
            return "v" + Constants.Version + "/token/session";
        }
    }
}
