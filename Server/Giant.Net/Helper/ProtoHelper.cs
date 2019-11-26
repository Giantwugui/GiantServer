using Google.Protobuf;
using System;
using System.IO;

namespace Giant.Net
{
    public static class ProtoHelper
    {
        public static byte[] ToBytes(object message)
        {
            return ((Google.Protobuf.IMessage)message).ToByteArray();
        }

        public static void ToStream(MemoryStream stream, object message)
        {
            ((Google.Protobuf.IMessage)message).WriteTo(stream);
        }

        public static object FromBytes(byte[] content, Type type)
        {
            object obj = Activator.CreateInstance(type);

            ((Google.Protobuf.IMessage)obj).MergeFrom(content);

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
