using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace UnloqAPI
{
    public class Utils
    {
        public static void PrepareHeaders(HttpRequestMessage message, string key, string secret)
        {
            message.Headers.Add("X-Api-Key", key);
            message.Headers.Add("X-Api-Secret", secret);
        }

        public static string CreateAuthorizeEndPoint()
        {
            return "v" + Constants.Version + "/authenticate";
        }

        public static string CreateUpdateHooksEndPoint()
        {
            return "v" + Constants.Version + "/settings/webhooks";
        }

        public static string CreateGetLoginTokenEndpoint()
        {
            return "v" + Constants.Version + "/token";
        }

        public static string CreateSetTokenDataEndpoint()
        {
            return "v" + Constants.Version + "/token/session";
        }

        public static string CreateUpdateAppLinkingEndPoint()
        {
            return "v" + Constants.Version + "/settings/linking";
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

        public static Dictionary<string, string> ConstructPayloadForUpdateHooks(string loginPath, string logoutPath)
        {
            return new Dictionary<string, string>
            {
                { "login", loginPath },
                { "logout", logoutPath }
            };
        }

        public static Dictionary<string, string> ConstructPayloadForUpdateAppLinking(string linkPath, string unlinkPath, bool disable)
        {
            return new Dictionary<string, string>
            {
                { "link", linkPath },
                { "unlink", unlinkPath },
                { "disable", disable.ToString() }
            };
        }

        public static async Task<UResponse> BuildUResponse(HttpResponseMessage response)
        {
            return await response.Content.ReadAsAsync<UResponse>();
        }

        public static async Task<UGetTokenResponse> BuildUGetTokenResponse(HttpResponseMessage response)
        {
            return await response.Content.ReadAsAsync<UGetTokenResponse>();
        }
    }
}
