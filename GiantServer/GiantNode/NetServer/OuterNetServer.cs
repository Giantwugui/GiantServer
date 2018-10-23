using GiantCore;
using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Concurrent;
using GiantNode.Interface;

namespace GiantNode
{
    /// <summary>
    /// 外部通讯服务
    /// </summary>
    public class OuterNetServer
    {
        public static void Init(IRunTime runTime)
        {
            int port = 0;

            mRunTime = runTime;

            if (!int.TryParse(runTime.GetParam("FrontPort"), out port))
            {
                throw new ArgumentNullException(string.Format("Node {0}_{1} FrontPort can't be null ", runTime.NodeName, runTime.NodeId));
            }

            mTcpListener = new TcpListener(new IPEndPoint(IPAddress.Any, port));

            mTcpListener.Start(3000);

            ThreadHelper.CreateThread(ListenLoop, "AcceptListen");
        }

        private static void ListenLoop()
        {
            try
            {
                Socket socket = mTcpListener.AcceptSocket();

                ServerSession serverSocket = new ServerSession(socket);

                serverSocket.OnClosed += OnClosed;

                serverSocket.OnReceiveMessage += OnReceiveMessage;

                if (!mSessionList.TryAdd(serverSocket.Uid, serverSocket))
                {
                    throw new Exception(string.Format("重复的客户端会话 Id {0}", serverSocket.Uid));
                }
            }
            catch (Exception ex)
            {
                Log.LogOut(LogType.Error, "ListenLoop Error " + ex.ToString());
            }

        }



        /// <summary>
        /// 客户端下线
        /// </summary>
        private static void OnClosed(Session session)
        {
            mSessionList.TryRemove(session.Uid, out ServerSession serverSocket);
        }

        /// <summary>
        /// 接受到来自客户端的消息
        /// </summary>
        private static void OnReceiveMessage(Session session, byte[] message)
        {
        }

        public static uint NodeId
        {
            get { return mRunTime.NodeId; }
        }

        /// <summary>
        /// 节店运行时环境
        /// </summary>
        private static IRunTime mRunTime = null;

        /// <summary>
        /// 客户端连接监听对象
        /// </summary>
        private static TcpListener mTcpListener = null;

        /// <summary>
        /// 回话对象
        /// </summary>
        private static ConcurrentDictionary<ulong, ServerSession> mSessionList = new ConcurrentDictionary<ulong, ServerSession>();
    }
}
