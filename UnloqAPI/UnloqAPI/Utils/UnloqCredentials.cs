namespace UnloqAPI
{
    public class UnloqCredentials
    {
        public string Email { get; set; }

        private string _token = "";
        public string Token 
        { 
            get { return _token; }
            set { _token = value; }
        }
    }
}
