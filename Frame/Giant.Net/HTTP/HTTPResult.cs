namespace Giant.Net
{
    public class HttpResult
    {
        public int code;
        public string msg;
        public object data;
    }

    public enum HttpErrorCode
    {
        Success = 0,
        Fail = 99,
    }
}
