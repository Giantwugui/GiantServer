using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Giant.Net
{
    public class HttpService
    {
        private HttpListener httpListener;

        public void Start(List<int> ports)
        {
            try
            {
                httpListener = new HttpListener();
                ports.ForEach(port => httpListener.Prefixes.Add($"http://*:{port}/"));
                httpListener.Start();

                ReceiveAsync();
            }
            catch (HttpListenerException e)
            {
                if (e.ErrorCode == 5)
                {
                    throw new Exception($"CMD管理员中输入: netsh http add urlacl url=http://*:8080/ user=Everyone", e);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async void ReceiveAsync()
        {
            var context = await httpListener.GetContextAsync();

            DoContext(context);

            ReceiveAsync();
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
