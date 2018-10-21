using ProtoBuf;

namespace GiantNode
{
    /// <summary>
    /// 外部消息
    /// </summary>
    [ProtoContract]
    public class OutMessage : IMessage
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
