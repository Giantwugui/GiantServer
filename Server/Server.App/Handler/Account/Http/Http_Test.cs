using Giant.Core;
using Giant.Logger;
using Giant.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Server.Account
{
    public class Http_Test
    {
        static readonly HttpComponent service = new HttpComponent();

        private static async void DoGet()
        {
            int i = 0;
            string url = "http://127.0.0.1:8080/GetVAsync";
            Stopwatch stopwatch = new Stopwatch();
            while (++i < 100)
            {
                await Task.Delay(1000);
                stopwatch.Restart();

                string result = await HttpHelper.GetAsync(url);
                stopwatch.Stop();

                Log.Debug($"get {result} cost {stopwatch.ElapsedMilliseconds}");
            }
        }

        private static async void DoPost()
        {
            try
            {
                int i = 0;
                string url = "http://127.0.0.1:8080/TestPostTime";
                Stopwatch stopwatch = new Stopwatch();
                while (++i < 100)
                {
                    await Task.Delay(1000);
                    stopwatch.Restart();

                    string result = await HttpHelper.PostAsync(url, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    stopwatch.Stop();

                    Log.Debug($"get {result} cost {stopwatch.ElapsedMilliseconds}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static async void DoPost1()
        {
            try
            {
                Dictionary<string, string> pairs = new Dictionary<string, string>
                {
                    { "Account", "wugui"},
                    { "Pass", "111111" }
                };

                string content = await HttpHelper.PostAsync("http://127.0.0.1:8080/", pairs.ToJson());
                Console.WriteLine(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}
