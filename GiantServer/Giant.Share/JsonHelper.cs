using Newtonsoft.Json;

namespace Giant.Share
{
    public static class JsonHelper
    {
        /// <summary>
        /// 将对象序列化为JSON格式
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>json字符串</returns>
        public static string ToJson(this object o)
        {
            if (null == o)
            {
                return string.Empty;
            }

            return o is string ? o.ToString() : JsonConvert.SerializeObject(o);
        }

        /// <summary>
        /// 解析JSON字符串生成对象实体
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串(eg.{"ID":"112","Name":"石子儿"})</param>
        /// <returns>对象实体</returns>
        public static T ToObject<T>(this string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
