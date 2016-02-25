namespace UnloqAPI.Utils
{
    public class UnloqOpts
    {
        private string _ip = "";
        public string Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }
        public UnloqMethod Method { get; set; }
    }
}
