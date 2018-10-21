using System;
using System.Net.Sockets;

namespace GiantCore
{
    public class ServerSocket : GSocket, IDisposable
    {
        public ServerSocket(Socket socket)
            : base(socket)
        {
            mUid = IDPool.GetID();
        }

        public void Send(byte[] message)
        {
            base.ToSend(new GBuffer(message));
        }

        public void Dispose()
        {
            IDPool.FreeID(mUid);
        }

        public override void ToStart()
        {
            base.ToStart();
        }

        protected override void NotifyClosed(SocketError error)
        {
            if (OnClosed != null)
            {
                OnClosed(mUid);
            }
        }

        protected override void NotifyReceived(byte[] message)
        {
            if (OnReceiveMessage != null)
            {
                OnReceiveMessage(mUid, message);
            }
        }

        /// <summary>
        /// 回话id
        /// </summary>
        public uint Uid
        {
            get { return mUid; }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public Action<uint> OnClosed = null;

        /// <summary>
        /// 收到消息
        /// </summary>
        public Action<uint, byte[]> OnReceiveMessage = null;

        /// <summary>
        /// 回话id
        /// </summary>
        private readonly uint mUid = 0;
    }
}
