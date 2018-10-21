using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace GiantNode
{
    public enum MessageType
    {
        /// <summary>
        /// 内部消息
        /// </summary>
        InnerMessage = 1,

        /// <summary>
        /// 客户端消息
        /// </summary>
        ClientMessagge = 2,
    }


    /// <summary>
    /// 内部消息
    /// </summary>
    [ProtoContract]
    public class InnerMessage : IMessage
    {
        public uint ToNode
        {
            get { return mToNodeId; }
            set { mToNodeId = value; }
        }

        public MessageType MessageType
        {
            get { return mMessageType; }
            set { mMessageType = value; }
        }

        public byte[] Content
        {
            get { return mContent; }
            set { mContent = value; }
        }

        /// <summary>
        /// 消息类型
        /// </summary>
        [ProtoMember(1)]
        private MessageType mMessageType;

        /// <summary>
        /// 发往节点id
        /// </summary>
        [ProtoMember(2)]
        private uint mToNodeId = 0;

        /// <summary>
        /// 消息内容
        /// </summary>
        [ProtoMember(3)]
        private byte[] mContent;
    }
}
