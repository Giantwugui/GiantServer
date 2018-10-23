using GiantCore;
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

        /// <summary>
        /// 发往节点id
        /// </summary>
        [ProtoMember(1)]
        private uint mToNodeId = 0;

        /// <summary>
        /// 消息内容
        /// </summary>
        [ProtoMember(2)]
        private byte[] mContent;
    }
}
