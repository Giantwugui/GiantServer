using System;
using ProtoBuf;

namespace Giant.Net
{
    /// <summary>
    /// 客户端会话信息类
    /// </summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class Session
    {
        public Session(uint nodeId, long uid)
        {
            Uid = uid;
            NodeId = nodeId;
        }

        /// <summary>
        /// 消息返回
        /// </summary>
        public void Return(string message)
        {
        }

        /// <summary>
        /// 节点id
        /// </summary>
        [ProtoMember(1)]
        public uint NodeId { get; private set; }

        /// <summary>
        /// 回话对象id
        /// </summary>
        [ProtoMember(2)]
        public long Uid { get; private set; }
    }
}
