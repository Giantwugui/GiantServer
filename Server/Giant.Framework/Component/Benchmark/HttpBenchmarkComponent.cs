using Giant.Core;
using Giant.Logger;
using Giant.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Giant.Framework
{
    public class HttpBenchmarkComponent : Component, IInitSystem
    {
        readonly int count = 100;

        public async void Get()
        {
            int i = 0;
            string url = "http://127.0.0.1:8080/GetVAsync";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (++i < count)
            {
                await HttpHelper.GetAsync(url);
            }

            stopwatch.Stop();
            Log.Debug($"Get count {count} cost {stopwatch.ElapsedMilliseconds} ms");
        }

        public async void Post()
        {
            try
            {
                int i = 0;
                string url = "http://127.0.0.1:8080/TestPostTime";
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                while (++i < count)
                {
                    await HttpHelper.PostAsync(url, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                }

                stopwatch.Stop();
                Log.Debug($"Post count {count} cost {stopwatch.ElapsedMilliseconds} ms");
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
