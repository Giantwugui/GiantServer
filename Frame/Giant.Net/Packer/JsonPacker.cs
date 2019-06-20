using System;
using System.IO;
using System.Text;
using Giant.Share;

namespace Giant.Net
{
    public class JsonPacker : IMessagePacker
    {
        public byte[] SerializeTo(object message)
        {
            return JsonHelper.ToBytes(message);
        }

        public void SerializeTo(MemoryStream stream, object message)
        {
            JsonHelper.ToStream(stream, message);
        }

        public object DeserializeFrom(string content, Type type)
        {
            return JsonHelper.FromBytes(Encoding.UTF8.GetBytes(content), type);
        }

        public object DeserializeFrom(byte[] content, Type type)
        {
            return JsonHelper.FromBytes(content, type);
        }

        public object DeserializeFrom(MemoryStream stream, Type type)
        {
            return JsonHelper.FromStream(stream, type);
        }
    }
}
