using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace Giant.Share
{
    public class JsonHelper
    {
        public static string ToJson(object message)
        {
            if (null == message)
            {
                return string.Empty;
            }

            return message is string ? message as string : JsonConvert.SerializeObject(message);
        }

        public static byte[] ToBytes(object message)
        {
            string jsonStr = ToJson(message);
            return Encoding.UTF8.GetBytes(jsonStr);
        }

        public static void ToStream(MemoryStream stream, object message)
        {
            byte[] content = ToBytes(message);
            stream.Write(content);
        }

        public static object FromBytes(byte[] content, Type type)
        {
            string jsonStr = Encoding.UTF8.GetString(content);
            return FromJson(jsonStr, type);
        }

        public static object FromStream(MemoryStream stream, Type type)
        {
            byte[] buffer = new byte[stream.Length - stream.Position];
            stream.Read(buffer);
            return FromBytes(buffer, type);
        }

        public static object FromJson(string json, Type type)
        {
            if (string.IsNullOrEmpty(json))
            {
                return default;
            }

            return JsonConvert.DeserializeObject(json, type);
        }
    }
}
