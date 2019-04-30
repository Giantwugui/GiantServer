using System;
using System.IO;
using System.Text;
using Google.Protobuf;

namespace Giant.Share
{
    /// <summary>
    /// protobuf 扩展类
    /// </summary>
    public static class ProtoHelper
    {
        public static byte[] ToBytes(object message)
        {
            return ((IMessage)message).ToByteArray();
        }


        public static void ToStream(object message)
        {

        }

        public static object FromBytes(byte[] content, Type type)
        {
            object obj = Activator.CreateInstance(type);

            ((IMessage)obj).MergeFrom(content);

            return obj;
        }

        public static T FromStream<T>(this MemoryStream stream) where T : IMessage
        {
            T obj = Activator.CreateInstance<T>();

            ((IMessage)obj).MergeFrom(stream);

            return obj;
        }

        public static object FromStream(MemoryStream stream, Type type)
        {
            object obj = Activator.CreateInstance(type);

            ((IMessage)obj).MergeFrom(stream);

            return obj;
        }

    }
}
