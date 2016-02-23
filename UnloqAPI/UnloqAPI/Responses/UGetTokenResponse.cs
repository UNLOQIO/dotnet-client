namespace UnloqAPI
{
    public class UGetTokenResponse : IUResponse
    {
        public string Type { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public GetTokenData Data { get; set; }
    }
}
