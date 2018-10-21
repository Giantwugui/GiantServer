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
    public class OutNetServer
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

        public ServerSocket GetSocket(uint uid)
        {
            if (mSocket.ContainsKey(uid))
            {
                return mSocket[uid];
            }

            return null;
        }


        private static void ListenLoop()
        {
            try
            {
                Socket socket = mTcpListener.AcceptSocket();

                ServerSocket serverSocket = new ServerSocket(socket);

                serverSocket.OnClosed += OnClosed;

                serverSocket.OnReceiveMessage += OnReceiveMessage;

                if (!mSocket.TryAdd(serverSocket.Uid, serverSocket))
                {
                    throw new Exception(string.Format("重复的客户端会话 Id {0}", serverSocket.Uid));
                }

                if (!mSessionList.TryAdd(serverSocket.Uid, new Session(mRunTime.NodeId, serverSocket.Uid)))
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
        private static void OnClosed(uint uid)
        {
            mSocket.TryRemove(uid, out ServerSocket serverSocket);

            mSessionList.TryRemove(uid, out Session session);
        }

        /// <summary>
        /// 接受到来自客户端的消息
        /// </summary>
        private static void OnReceiveMessage(uint uid, byte[] message)
        {
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
        /// 通讯对象
        /// </summary>
        private static ConcurrentDictionary<uint, ServerSocket> mSocket = new ConcurrentDictionary<uint, ServerSocket>();

        /// <summary>
        /// 回话对象
        /// </summary>
        private static ConcurrentDictionary<uint, Session> mSessionList = new ConcurrentDictionary<uint, Session>();
    }
}
