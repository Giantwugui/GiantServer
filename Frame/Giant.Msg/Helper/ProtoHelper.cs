using System;
using System.IO;
using Google.Protobuf;

namespace Giant.Message
{
    /// <summary>
    /// protobuf 扩展类
    /// </summary>
    public static class ProtoHelper
    {
        public static byte[] ToBytes(object message)
        {
            return ((Google.Protobuf.IMessage)message).ToByteArray();
        }

        public static void ToStream(MemoryStream memoryStream, object message)
        {
            ((Google.Protobuf.IMessage)message).WriteTo(memoryStream);
        }

        public static object FromBytes(byte[] content, Type type)
        {
            object obj = Activator.CreateInstance(type);

            ((Google.Protobuf.IMessage)obj).MergeFrom(content);

            return obj;
        }

        public static T FromStream<T>(this MemoryStream stream) where T : Google.Protobuf.IMessage
        {
            T obj = Activator.CreateInstance<T>();

            ((Google.Protobuf.IMessage)obj).MergeFrom(stream);

            return obj;
        }

        public static object FromStream(MemoryStream stream, Type type)
        {
            object obj = Activator.CreateInstance(type);

            ((Google.Protobuf.IMessage)obj).MergeFrom(stream);

            return obj;
        }

    }
}
