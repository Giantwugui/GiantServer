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
        public Session() { }

        public Socket ClientSocket
        {
            get { return mSocket; }
            set { this.mSocket = value; }
        }

        public void SetSocket(Socket socket)
        {
            mSocket = socket;
        }

        /// <summary>
        /// 消息返回
        /// </summary>
        public void Return(string message)
        {
            mSocket.Send(Encoding.Unicode.GetBytes(message));
        }

        private Socket mSocket;
    }
}
