using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;
using Giant.Net;

namespace Giant.Framework
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

            mThread = new Thread(AcceptLoop);
            mThread.Name = "Accept";
            mThread.IsBackground = true;
            mThread.Start();
        }


        private static void AcceptLoop()
        {
            try
            {
                while (true)
                {
                    TCPChannel channel = new TCPChannel(mTcpListener.AcceptSocket());

                    channel.ErrorCallBack += OnClosed;
                    channel.ReceiveCallBack += OnReceiveMessage;

                    if (!mChannelList.TryAdd(channel.Id, channel))
                    {
                        throw new Exception($"重复的客户端会话 Id {channel.Id}");
                    }

                    Session session = new Session(mRunTime.NodeId, channel.Id);

                    mSessionList.TryAdd(session.Id, session);

                    MessageManager.Add(new Message(MessageType.ClientOnline, session));

                    channel.Start();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"mAccept error {ex.ToString()}");
            }
        }


        /// <summary>
        /// 客户端下线
        /// </summary>
        private static void OnClosed(BChannel channel, SocketError error)
        {
            mChannelList.Remove(channel.Id);
            mSessionList.Remove(channel.Id, out Session session);

            MessageManager.Add(new Message(MessageType.ClientOffline, session));
        }

        /// <summary>
        /// 接受到来自客户端的消息
        /// </summary>
        private static void OnReceiveMessage(BChannel channel, byte[] message)
        {
            mSessionList.TryGetValue(channel.Id, out Session session);

            MessageManager.Add(new Message(MessageType.Client, session, message));
        }

        public static uint NodeId
        {
            get { return mRunTime.NodeId; }
        }

        private static Thread mThread = null;

        /// <summary>
        /// 节店运行时环境
        /// </summary>
        private static IRunTime mRunTime = null;

        /// <summary>
        /// 客户端连接监听对象
        /// </summary>
        private static TcpListener mTcpListener = null;

        /// <summary>
        /// 连接对象
        /// </summary>
        private static Dictionary<long, TCPChannel> mChannelList = new Dictionary<long, TCPChannel>();

        /// <summary>
        /// 回话对象
        /// </summary>
        private static Dictionary<long, Session> mSessionList = new Dictionary<long, Session>();
    }
}
