using System;
using System.Collections.Generic;
using System.Text;

namespace GiantNode
{
    public interface IMessage
    {
        uint ToNode { get; }

        MessageType MessageType { get; }

        byte[] Content { get; }
    }
}
