using System.Collections.Generic;
using System;
using Giant.Net;
using NetMQ.Sockets;
using NetMQ;
using Giant.Core;
using System.Threading;

namespace Giant.Framework
{
    /// <summary>
    /// 内部部通讯服务
    /// </summary>
    public class InnerNetService
    {
        public static void Init(IRunTime runTime)
        {
            mRunTime = runTime;

            //mPuller.Bind($"ipc://NodeServer_{runTime.GroupId}_{runTime.NodeId}");
            mPuller.Bind($"ipc://NodeServer_1_1");

            mThread = new Thread(ReceiveLoop);
            mThread.Name = "Receive";
            mThread.IsBackground = true;
            mThread.Start();

            for (int i = 0; i < runTime.Nodes.Length; ++i)
            {
                if (runTime.Nodes[i] != runTime.NodeId)
                {
                    mPublisher[runTime.Nodes[i]] = new PublisherSocket($"ipc://GiantNode_{runTime.NodeId}_{runTime.Nodes[i]}");
                }
            }
        }

        /// <summary>
        /// 转发消息
        /// </summary>
        public static void Transmit(uint toNode, string message)
        {
            mPublisher[toNode].SendFrame(message);
        }


        private static void ReceiveLoop()
        {
            while (true)
            {
                try
                {
                    InnerMessage message = mPuller.ReceiveFrameString().ToProtoObject<InnerMessage>();

                    MessageManager.Add(new Message(MessageType.Inner, message));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private static Thread mThread = null;

        private static IRunTime mRunTime = null;

        /// <summary>
        /// 消息接受
        /// </summary>
        private static PullSocket mPuller = new PullSocket();

        /// <summary>
        /// 其他节点内部通讯对象
        /// </summary>
        private static Dictionary<uint, PublisherSocket> mPublisher = new Dictionary<uint, PublisherSocket>();
    }
}
