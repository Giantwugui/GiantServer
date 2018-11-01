using System;
using ProtoBuf;

namespace GiantCore
{
    /// <summary>
    /// 客户端会话信息类
    /// </summary>
    [ProtoContract]
    public class Session
    {
        public Session(uint nodeId, ulong uid)
        {
            mUid = uid;
            mNodeId = nodeId;
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
        public uint NodeId
        {
            get { return mNodeId; }
            set { mNodeId = value; }
        }

        /// <summary>
        /// 回话对象id
        /// </summary>
        public ulong Uid
        {
            get { return mUid; }
            set { mUid = value; }
        }

        /// <summary>
        /// 节点id
        /// </summary>
        [ProtoMember(1)]
        private uint mNodeId = 0;

        /// <summary>
        /// 回话对象id
        /// </summary>
        [ProtoMember(2)]
        private ulong mUid = 0;
    }
}
