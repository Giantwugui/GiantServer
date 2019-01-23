using Giant.Net;
using System;

namespace Giant.Framework
{
    public interface IMessage
    {
        uint Uid { get; }

        uint ToNode { get; }

        MessageType MessageType { get; }

        byte[] Content { get; }
    }
}
