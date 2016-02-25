namespace UnloqAPI.Utils
{
    public class SessionData
    {
        private string _sessionId = "";
        public string SessionId
        {
            get { return _sessionId; }
            set { _sessionId = value; }
            
        }

        private int? _duration;
        public int? Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }
    }
}
