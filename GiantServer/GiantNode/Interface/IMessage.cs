using System;

namespace GiantNode
{
    public interface IMessage
    {
        uint ToNode { get; }

        MessageType MessageType { get; }

        byte[] Content { get; }
    }
}
