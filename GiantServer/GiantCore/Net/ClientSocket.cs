using System;
using System.Net.Sockets;

namespace GiantCore
{
    /// <summary>
    /// 客户端连接类
    /// </summary>
    public class ClientSocket : GSocket
    {
        public ClientSocket(string host, int port)
            : base(host, port)
        {          
        }

        public void Send(byte[] message)
        {
            base.ToSend(new GBuffer(message));
        }

        public override void ToStart()
        {
            base.ToStart();
        }

        protected override void NotifyClosed(SocketError error)
        {
            if (OnClosed != null)
            {
                OnClosed();
            }
        }

        protected override void NotifyConnected(bool isConnected)
        {
            if (OnConnected != null)
            {
                OnConnected(isConnected);
            }
        }

        protected override void NotifyReceived(byte[] message)
        {
            if (OnReceiveMessage != null)
            {
                OnReceiveMessage(message);
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public Action OnClosed = null;

        /// <summary>
        /// 连接成功
        /// </summary>
        public Action<bool> OnConnected = null;

        /// <summary>
        /// 收到消息
        /// </summary>
        public Action<byte[]> OnReceiveMessage = null;
    }
}
