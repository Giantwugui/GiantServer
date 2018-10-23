using ProtoBuf;

namespace GiantNode
{
    /// <summary>
    /// 外部消息
    /// </summary>
    [ProtoContract]
    public class OuterMessage
    {
        public uint ToNode
        {
            get { return mToNodeId; }
            set { mToNodeId = value; }
        }

        public byte[] Content
        {
            get { return mContent; }
            set { mContent = value; }
        }

        public uint Uid
        {
            get { return mUid; }
            set { mUid = value; }
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
        /// 客户端id
        /// </summary>
        [ProtoMember(3)]
        private uint mUid = 0;

        /// <summary>
        /// 消息内容
        /// </summary>
        [ProtoMember(4)]
        private byte[] mContent;
    }
}
