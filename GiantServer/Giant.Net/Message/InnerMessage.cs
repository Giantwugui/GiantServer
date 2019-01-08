using Giant.Core;
using ProtoBuf;
using System.Collections.Generic;

namespace Giant.Net
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
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class InnerMessage
    {
        public InnerMessage()
        { }

        public InnerMessage(uint toNode, InnerMessageType messageType, byte[] content)
        {
            ToNodeId = toNode;
            MessageType = messageType;
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
        public InnerMessageType MessageType { get; private set; }

        /// <summary>
        /// 发往节点id
        /// </summary>
        public uint ToNodeId { get; private set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        private Dictionary<string, byte[]> mContent = new Dictionary<string, byte[]>();
    }
}
