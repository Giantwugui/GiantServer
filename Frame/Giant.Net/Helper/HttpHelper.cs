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
        static readonly char[] splitParam = new char[] { '&' };
        static readonly char[] splitKV = new char[] { '=' };

        /// <summary>
        /// 使用post方法异步请求
        /// </summary>
        /// <param name="url">目标链接</param>
        /// <param name="message">发送的参数字符串</param>
        /// <returns>返回的字符串</returns>
        public static async Task<string> PostAsync(string url, string message)
        {
            using HttpClient client = new HttpClient();
            return await PostAsync(client, url, message);
        }

        public static async Task<string> PostAsync(HttpClient client, string url, string message)
        {
            HttpContent content = new StringContent(message);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            HttpResponseMessage response = await client.PostAsync(url, content);
            string result = await response.Content.ReadAsStringAsync();
            response.Dispose();

            return result;
        }

        /// <summary>
        /// 使用post方法异步请求
        /// </summary>
        /// <param name="url">目标链接</param>
        /// <param name="message">http头</param>
        /// <returns></returns>
        public static async Task<string> PostAsync(string url, Dictionary<string, string> message)
        {
            using HttpClient client = new HttpClient();
            return await PostAsync(client, url, message);
        }

        public static async Task<string> PostAsync(HttpClient client, string url, Dictionary<string, string> message)
        {
            HttpContent content = new FormUrlEncodedContent(message);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            HttpResponseMessage response = await client.PostAsync(url, content);
            string result = await response.Content.ReadAsStringAsync();
            response.Dispose();

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
            response.Dispose();
            client.Dispose();
            return result;
        }

        public static async Task<string> GetAsync(string url, Dictionary<string, string> head = null)
        {
            using HttpClient client = new HttpClient();
            return await GetAsync(client, url, head);
        }

        public static async Task<string> GetAsync(HttpClient client, string url, Dictionary<string, string> head = null)
        {
            url = BuildUrl(url, head);
            HttpResponseMessage response = await client.GetAsync(url);
            string result = await response.Content.ReadAsStringAsync();
            response.Dispose();

            return result;
        }

        public static Dictionary<string, string> ParaseParams(string url)
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
                    foreach (var curr in paramStr.Split(splitParam, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string[] kv = curr.Split(splitKV);
                        if (kv.Length != 2) continue;

                        param[kv[0]] = kv[1];
                    }
                }
            }

            return param;
        }

        public static Dictionary<string, string> ParaseContent(string message)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(message))
            {
                foreach (var curr in message.Split(splitParam, StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] kv = curr.Split(splitKV);
                    if (kv.Length != 2) continue;

                    param[kv[0]] = kv[1];
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
            if (pairs == null)
            {
                return url;
            }

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
            return string.Join("&", kvs);
        }
    }
}
