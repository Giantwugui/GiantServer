using System;
using System.IO;
using System.Text;

namespace Giant.Share
{
    public class ProtoPacker : IMessagePacker
    {
        public byte[] SerializeTo(object message)
        {
            return ProtoHelper.ToBytes(message);
        }

        public void SerializeTo(MemoryStream stream, object message)
        {
            ProtoHelper.ToStream(stream, message);
        }

        public object DeserializeFrom(string content, Type type)
        {
            return ProtoHelper.FromBytes(Encoding.UTF8.GetBytes(content), type);
        }

        public object DeserializeFrom(byte[] content, Type type)
        {
            return ProtoHelper.FromBytes(content, type);
        }

        public object DeserializeFrom(MemoryStream stream, Type type)
        {
            return ProtoHelper.FromStream(stream, type);
        }
    }
}
