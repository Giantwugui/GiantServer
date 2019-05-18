using System;
using System.IO;

namespace Giant.Msg
{
    public interface IMessagePacker
    {
        byte[] SerializeTo(object message);
        void SerializeTo(MemoryStream stream, object message);
        object DeserializeFrom(string content, Type type);
        object DeserializeFrom(byte[] content, Type type);
        object DeserializeFrom(MemoryStream stream, Type type);
    }
}
