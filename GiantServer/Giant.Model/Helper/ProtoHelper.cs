using System.IO;
using System.Text;
using ProtoBuf;

namespace Giant.Model.Helper
{
    /// <summary>
    /// protobuf 扩展类
    /// </summary>
    public static class ProtoHelper
    {
        public static string ToProtoString<T>(this T self) where T : class
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Serializer.Serialize<T>(stream, self);

                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        public static byte[] ToProtoBytes<T>(this T self) where T : class
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Serializer.Serialize<T>(stream, self);

                return stream.ToArray();
            }
        }

        public static T ToProtoObject<T>(this string content)
        {
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                return Serializer.Deserialize<T>(stream);
            }
        }

        public static T ToProtoObject<T>(this byte[] content)
        {
            using (MemoryStream stream = new MemoryStream(content))
            {
                return Serializer.Deserialize<T>(stream);
            }
        }
    }
}
