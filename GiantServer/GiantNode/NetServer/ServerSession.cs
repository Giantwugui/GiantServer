using System;
using System.Collections.Generic;
using System.Net.Sockets;
using GiantCore;

namespace GiantNode
{
    public class ServerSession : GSocket, IDisposable
    {
        public ServerSession(Socket socket)
            : base(socket)
        {
            mSession = new Session(OuterNetServer.NodeId, GetID());
        }

        public void Send(byte[] message)
        {
            base.ToSend(new GBuffer(message));
        }

        public void Dispose()
        {
            FreeID(mSession.Uid);
        }

        public override void ToStart()
        {
            base.ToStart();
        }

        protected override void NotifyClosed(SocketError error)
        {
            if (OnClosed != null)
            {
                OnClosed(mSession);
            }
        }

        protected override void NotifyReceived(byte[] message)
        {
            if (OnReceiveMessage != null)
            {
                OnReceiveMessage(mSession, message.ToProtoObject<OuterMessage>());
            }
        }

        private static ulong GetID()
        {
            lock (mIDPool)
            {
                uint id = 0;
                while (mIDPool.Contains(++id))
                {
                }
                return id;
            }
        }

        private static void FreeID(ulong id)
        {
            lock (mIDPool)
            {
                if (mIDPool.Contains(id))
                {
                    mIDPool.Remove(id);
                }
            }
        }

        /// <summary>
        /// 会话id
        /// </summary>
        public ulong Uid
        {
            get { return mSession.Uid; }
        }

        public Session Session
        {
            get { return mSession; }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public Action<Session> OnClosed = null;

        /// <summary>
        /// 收到消息
        /// </summary>
        public Action<Session, OuterMessage> OnReceiveMessage = null;

        /// <summary>
        /// 回话标识
        /// </summary>
        private readonly Session mSession = null;


        private static HashSet<ulong> mIDPool = new HashSet<ulong>();

    }
}
