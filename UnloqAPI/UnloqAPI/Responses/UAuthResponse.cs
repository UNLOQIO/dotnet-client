namespace UnloqAPI
{
    public class UAuthResponse : IUResponse
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
        public AuthData Data { get; set; }
    }
}
