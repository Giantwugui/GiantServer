using GiantCore;
using GiantNode.Interface;
using NetMQ;
using NetMQ.Sockets;

namespace GiantNode
{
    /// <summary>
    /// 内部消息管理类
    /// </summary>
    public class InnerNetServer
    {
        public static void Init(IRunTime runTime)
        {
            mPuller.Bind(string.Format("ipc://GiantNode_{0}_{1}", runTime.GroupId, runTime.NodeId));

            ThreadHelper.CreateThread(ReceiveLoop, "Receive");

            if (runTime.IsFrontNode)
            {
                ThreadHelper.CreateThread(ListenLoop, "ListenConn");
            }
        }


        private static void ReceiveLoop()
        {
            try
            {
                mPuller.ReceiveFrameBytes();
            }
            catch
            {
            }
        }

        private static void ListenLoop()
        {
        }



        private static PullSocket mPuller = new PullSocket();
    }
}
