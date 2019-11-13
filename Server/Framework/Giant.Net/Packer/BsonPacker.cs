using Giant.Share;
using System;
using System.IO;
using System.Text;

namespace Giant.Net
{
    public class BsonPacker : IMessagePacker
    {
        public byte[] SerializeTo(object message)
        {
            return JsonHelper.ToJsonBytes(message);
        }

        public void SerializeTo(MemoryStream stream, object message)
        {
            JsonHelper.ToStream(stream, message);
        }

        public object DeserializeFrom(string content, Type type)
        {
            return JsonHelper.FromJsonBytes(Encoding.UTF8.GetBytes(content), type);
        }

        public object DeserializeFrom(byte[] content, Type type)
        {
            return JsonHelper.FromJsonBytes(content, type);
        }

        public object DeserializeFrom(MemoryStream stream, Type type)
        {
            return JsonHelper.FromJsonStream(stream, type);
        }
    }
}
