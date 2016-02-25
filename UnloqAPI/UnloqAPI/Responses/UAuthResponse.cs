using UnloqAPI.Utils;

namespace UnloqAPI.Responses
{
    public class UResponse : IUResponse
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
        public UData Data { get; set; }
    }
}
