using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace GiantCore
{
    /// <summary>
    /// 客户端会话信息类
    /// </summary>
    public class Session
    {
        public Session(uint nodeId, uint uid)
        {
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
        public uint Uid
        {
            get { return mUid; }
            set { mUid = value; }
        }

        /// <summary>
        /// 节点id
        /// </summary>
        private uint mNodeId = 0;

        /// <summary>
        /// 回话对象id
        /// </summary>
        private uint mUid = 0;
    }
}
