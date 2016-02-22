using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
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

        public static Dictionary<string, string> ConstructPayload(string email, UnloqMethod method, string token,
            string ip = "")
        {
            return new Dictionary<string, string>
            {
               { "email", email },
               { "method", method.ToString() },
               { "ip", ip },
               { "token", token }
            };
        }

        public static async Task<UResponse> BuildUResponse(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var tokens = Regex.Split(content, "\"");

            if (tokens[Constants.TypeIndex] == "success")
            {
                return new USuccessResponse { Type = "success", Token = tokens[Constants.TokenIndex], Id = tokens[Constants.IdIndex].Substring(1, tokens[8].Length - 2) };
            }
            return new UErrorResponse { Code = tokens[Constants.CodeIndex], Message = tokens[Constants.MessageIndex], Type = "error" };
        }
    }
}
