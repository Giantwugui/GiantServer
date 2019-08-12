using Giant.Log;
using Giant.Net;
using Giant.Share;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Server.Test
{
    public class Http_Test
    {
        static readonly HttpService service = new HttpService();

        public static void Test()
        {
            service.Start(new List<int>() { 8080, 8081, 8082 });
            service.Load(Assembly.GetCallingAssembly());
        }

        private static void OnCmd(string content)
        {
            switch (content)
            {
                case "post":
                    {
                        DoPost();
                    }
                    break;
                case "post1":
                    {
                        DoPost1();
                    }
                    break;
                case "get":
                    {
                        DoGet();
                    }
                    break;
            }
        }

        private static async void DoPost()
        {
            try
            {
                Stopwatch stopwatch1 = new Stopwatch();
                stopwatch1.Start();

                for (int i = 0; i < 1000; ++i)
                {
                    Dictionary<string, string> pairs = new Dictionary<string, string>
                    {
                        { "Account", "wugui"+i },
                        { "Pass", "111111" }
                    };

                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    string content = await HttpHelper.PostAsync("http://127.0.0.1:8080/", pairs);

                    stopwatch.Stop();
                    Logger.Info($"Uid,wugui{i},coustTime,{stopwatch.ElapsedMilliseconds}");
                }

                stopwatch1.Stop();
                Console.WriteLine("Finish time total: " + stopwatch1);
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

        private static async void DoGet()
        {
            try
            {
                Dictionary<string, string> pairs = new Dictionary<string, string>
                {
                    { "Account", "wugui" },
                    { "Pass", "111111" },
                    { "Methord", "GET" }
                };

                string content = await Giant.Net.HttpHelper.GetAsync("http://127.0.0.1:8080/", pairs);
                Console.WriteLine(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}
