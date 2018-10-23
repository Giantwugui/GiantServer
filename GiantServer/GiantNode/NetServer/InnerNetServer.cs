using GiantCore;
using GiantNode.Interface;
using NetMQ;
using NetMQ.Sockets;
using System.Collections.Generic;
using System;

namespace GiantNode
{
    /// <summary>
    /// 内部部通讯服务
    /// </summary>
    public class InnerNetServer
    {
        public static void Init(IRunTime runTime)
        {
            mRunTime = runTime;

            mPuller.Bind(string.Format("ipc://NodeServer_1_1", runTime.GroupId, runTime.NodeId));

            ThreadHelper.CreateThread(ReceiveLoop, "Receive");

            for (int i = 0; i < runTime.Nodes.Length; ++i)
            {
                if (runTime.Nodes[i] != runTime.NodeId)
                {
                    mPublisher[runTime.Nodes[i]] = new PublisherSocket(string.Format("ipc://GiantNode_{0}_{1}", runTime.NodeId, runTime.Nodes[i]));
                }
            }
        }

        /// <summary>
        /// 转发消息
        /// </summary>
        public static void Transmit(uint toNode, Message message)
        {
            mPublisher[toNode].SendFrame(message.ToJson());
        }
       

        private static void ReceiveLoop()
        {
            while (true)
            {
                try
                {
                    InnerMessage message = mPuller.ReceiveFrameString().ToObject<InnerMessage>();

                    MessageManager.Enqueue(new Message(message.MessageType, message.Content));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


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
