using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ProtoBuf;

namespace GiantNode
{
    public static class ExternedClass
    {
        /// <summary>
        /// 序列化为标准json
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="self">对象</param>
        /// <returns></returns>
        public static string ToJson<T>(this T self) where T : class
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Serializer.Serialize<T>(stream, self);

                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// 反序列化为对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="content">反序列化字符</param>
        /// <returns></returns>
        public static T ToObject<T>(this string content)
        {
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                return Serializer.Deserialize<T>(stream);
            }
        }

        /// <summary>
        /// 反序列化为对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="content">反序列化字符</param>
        /// <returns></returns>
        public static T ToObject<T>(this byte[] content)
        {
            using (MemoryStream stream = new MemoryStream(content))
            {
                return Serializer.Deserialize<T>(stream);
            }
        }
    }
}
