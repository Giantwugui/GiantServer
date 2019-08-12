using Giant.Net;
using Giant.Share;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Test
{
    [HttpHandler(AppType.AllServer, "/")]
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

        [Get]
        public async Task<int> GetVAsync()
        {
            await Task.Delay(5000);
            return 5000;
        }
    }
}
