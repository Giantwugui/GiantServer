using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Giant.Net
{
    public class HttpService
    {
        private HttpListener httpListener;

        public void Start(int port)
        {
            httpListener = new HttpListener();
            httpListener.Prefixes.Add($"http://*:{port}/");
            httpListener.Start();

            Receive();
        }

        private async void Receive()
        {
            var context = await httpListener.GetContextAsync();

            DoContext(context);

            Receive();
        }

        private async void DoContext(HttpListenerContext context)
        {
            try
            {
                string timeStr = DateTime.Now.ToString();

                if (context.Request.RawUrl.Contains("favicon.ico"))
                {
                    context.Response.Close();
                    context.Response.Abort();
                    return;
                }


                Dictionary<string, string> param = new Dictionary<string, string>();

                if (context.Request.HttpMethod == "GET")
                {
                    //var nameValues = HttpUtility.ParseQueryString(context.Request.Url.Query, context.Request.ContentEncoding);
                    foreach (string key in context.Request.Headers)
                    {
                        param.Add(key, context.Request.Headers[key]);
                    }
                }
                else //"POST"
                {
                    using (StreamReader reader = new StreamReader(context.Request.InputStream))
                    {
                        string content = await reader.ReadToEndAsync();

                        Console.WriteLine($"{timeStr} read content: {content}");
                    }
                }

                using (StreamWriter stream = new StreamWriter(context.Response.OutputStream))
                {
                    await stream.WriteAsync(timeStr);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

    }
}
