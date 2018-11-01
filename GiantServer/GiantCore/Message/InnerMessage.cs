using ProtoBuf;
using System.Collections.Generic;

namespace GiantCore
{
    /// <summary>
    /// 内部消息类型
    /// </summary>
    public enum InnerMessageType
    {
        /// <summary>
        /// 内部消息
        /// </summary>
        Inner = 1,

        /// <summary>
        /// 客户端消息
        /// </summary>
        Client = 1<<1,

        /// <summary>
        /// 上线消息
        /// </summary>
        ClientOnline = 1<<2,

        /// <summary>
        /// 下线消息
        /// </summary>
        ClientOffline = 1<<3,

    }


    /// <summary>
    /// 内部消息
    /// </summary>
    [ProtoContract]
    public class InnerMessage
    {
        public InnerMessage()
        { }

        public InnerMessage(uint toNode, InnerMessageType messageType, byte[] content)
        {
            mToNodeId = toNode;
            mMessageType = messageType;
            mContent["Content"] = content;
        }

        public void Add<T>(string key, T o) where T : class
        {
            mContent[key] = o.ToProtoBytes();
        }

        public T Get<T>(string key) where T : class
        {
            return mContent.ContainsKey(key) ? mContent[key].ToProtoObject<T>() : null;
        }

        public uint ToNode
        {
            get { return mToNodeId; }
            set { mToNodeId = value; }
        }

        public InnerMessageType MessageType
        {
            get { return mMessageType; }
            set { mMessageType = value; }
        }

        public byte[] Content
        {
            get
            {
                return mContent["Content"];
            }
            set
            {
                mContent["Content"] = value;
            }
        }

        /// <summary>
        /// 消息类型
        /// </summary>
        [ProtoMember(1)]
        private InnerMessageType mMessageType;

        /// <summary>
        /// 发往节点id
        /// </summary>
        [ProtoMember(2)]
        private uint mToNodeId = 0;

        /// <summary>
        /// 消息内容
        /// </summary>
        [ProtoMember(3)]
        private Dictionary<string, byte[]> mContent = new Dictionary<string, byte[]>();
    }
}
