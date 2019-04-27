using Giant.Share;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Giant.Net
{
    public static class HttpHelper
    {
        static readonly char[] splitChar = new char[] { '&' };

        //public static async Task<string> PostAsync(string url, Dictionary<string, string> pairs)
        //{
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //    request.Method = "POST";
        //    request.ContentType = "application/x-www-form-urlencoded";
        //    request.Timeout = 5000;

        //    string sendData = BuildParams(pairs);
        //    request.ContentLength = sendData.Length;

        //    using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
        //    {
        //        await writer.WriteAsync(sendData);
        //    }

        //    HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;

        //    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
        //    {
        //        string content = await reader.ReadToEndAsync();

        //        return content;
        //    }
        //}

        /// <summary>
        /// 使用post方法异步请求
        /// </summary>
        /// <param name="url">目标链接</param>
        /// <param name="message">发送的参数字符串，只能用json</param>
        /// <param name="head">http头</param>
        /// <returns>返回的字符串</returns>
        public static async Task<string> PostAsync(string url, string message, Dictionary<string, string> head = null)
        {
            HttpClient client = new HttpClient();
            HttpContent content = new StringContent(message);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            if (head != null)
            {
                head.ForEach(kv => client.DefaultRequestHeaders.Add(kv.Key, kv.Value));
            }

            HttpResponseMessage response = await client.PostAsync(url, content);
            string result = await response.Content.ReadAsStringAsync();
            return result;
        }

        /// <summary>
        /// 使用post方法异步请求
        /// </summary>
        /// <param name="url">目标链接</param>
        /// <param name="json">发送的参数字符串，只能用json</param>
        /// <returns>返回的字符串</returns>
        public static async Task<string> PostAsyncJson(string url, string json)
        {
            HttpClient client = new HttpClient();
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            return result;
        }

        /// <summary>
        /// 使用post方法异步请求
        /// </summary>
        /// <param name="url">目标链接</param>
        /// <param name="head">http头</param>
        /// <returns></returns>
        public static async Task<string> PostAsync(string url, Dictionary<string, string> head)
        {
            HttpClient client = new HttpClient();
            HttpContent content = new FormUrlEncodedContent(head);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            head.ForEach(kv => client.DefaultRequestHeaders.Add(kv.Key, kv.Value));

            HttpResponseMessage response = await client.PostAsync(url, content);
            string result = await response.Content.ReadAsStringAsync();
            return result;
        }

        public static async Task<string> GetAsync(string url, Dictionary<string, string> head)
        {
            HttpClient client = new HttpClient();
            head.ForEach(kv => client.DefaultRequestHeaders.Add(kv.Key, kv.Value));

            HttpResponseMessage response = await client.GetAsync(url);
            string result = await response.Content.ReadAsStringAsync();
            return result;
        }


        public static Dictionary<string, string> ParaseParmas(string url)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();

            int startIndex = url.IndexOf("?");
            int endIndex = url.IndexOf("#");

            if (endIndex < 0)//没有结束标志位置
            {
                endIndex = url.Length;
            }

            if (startIndex > 0)
            {
                string paramStr = url.Substring(startIndex + 1, endIndex - startIndex - 1);
                if (!string.IsNullOrEmpty(paramStr))
                {
                    foreach (var curr in paramStr.Split(splitChar, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string[] kv = curr.Split('=');
                        if (kv.Length != 2) continue;

                        param[kv[0]] = kv[1];
                    }
                }
            }

            return param;
        }

        /// <summary>
        /// 构建带参数url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string BuildUrl(string url, Dictionary<string, string> pairs)
        {
            string paramStr = BuildParams(pairs);

            return $"{url}?{paramStr}";
        }

        /// <summary>
        /// 构建参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string BuildParams(Dictionary<string, string> param)
        {
            List<string> kvs = new List<string>();

            param.ForEach(kv => kvs.Add($"{kv.Key}={kv.Value}"));

            string paramStr = string.Join("&", kvs);

            return paramStr;
        }
    }
}
