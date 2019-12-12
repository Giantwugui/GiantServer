using Giant.Net;
using System;
using System.Threading.Tasks;

namespace Server.Account
{
    [HttpHandler("/")]
    public class HttpTestHandler : BaseHttpHandler
    {
        [Get]
        public static int Get()
        {
            return 1;
        }

        [Get]
        public static int GetInt(string name)
        {
            return 1;
        }

        [Post]
        public static string TestPostTime(string content)
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        [Get]
        public async Task<HttpResult> GetVAsync()
        {
            await Task.Delay(1);
            return Success(DateTime.Now);
        }
    }
}
