using System;

namespace GiantNode
{
    public interface IMessage
    {
        uint Uid { get; }

        uint ToNode { get; }

        MessageType MessageType { get; }

        byte[] Content { get; }
    }
}
