using GiantCore;
using System;
using System.Net;
using System.Net.Sockets;
using GiantNode.Interface;
using System.Collections.Generic;

namespace GiantNode
{
    /// <summary>
    /// 外部通讯服务
    /// </summary>
    public class OuterNetServer
    {
        public static void Init(IRunTime runTime)
        {
            mRunTime = runTime;

            if (!int.TryParse(runTime.GetParam("FrontPort"), out int port))
            {
                throw new ArgumentNullException($"Node {runTime.NodeName}_{runTime.NodeId} FrontPort 不能为空 ");
            }

            mTcpListener = new TcpListener(IPAddress.Any, port);
            mTcpListener.Start(3000);

            ThreadHelper.CreateThread(AcceptLoop, "Accept");
        }


        private static void AcceptLoop()
        {
            try
            {
                while (true)
                {
                    TSocket serverSocket = new TSocket(mTcpListener.AcceptSocket());

                    serverSocket.OnClosed += OnClosed;

                    serverSocket.OnReceiveMessage += OnReceiveMessage;

                    if (!mSessionList.TryAdd(serverSocket.Uid, serverSocket))
                    {
                        throw new Exception($"重复的客户端会话 Id {serverSocket.Uid}");
                    }

                    MessageManager.Add(new Message(MessageType.ClientOnline, serverSocket.Session));

                    serverSocket.ToStart();
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"mAccept error {ex.ToString()}");
            }
        }


        /// <summary>
        /// 客户端下线
        /// </summary>
        private static void OnClosed(Session session)
        {
            mSessionList.Remove(session.Uid, out TSocket serverSession);

            MessageManager.Add(new Message(MessageType.ClientOffline, session));

            serverSession.Dispose();
        }

        /// <summary>
        /// 接受到来自客户端的消息
        /// </summary>
        private static void OnReceiveMessage(Session session, OuterMessage message)
        {
            MessageManager.Add(new Message(MessageType.Client, session, message));
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
        private static Dictionary<ulong, TSocket> mSessionList = new Dictionary<ulong, TSocket>();
    }
}
