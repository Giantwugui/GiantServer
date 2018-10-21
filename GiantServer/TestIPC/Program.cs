using NetMQ.Sockets;
using NetMQ;
using System;
using System.Threading;

namespace TestIPC
{
    class Program
    {
        private static void ReceivceLoop(object param)
        {
            while (true)
            {
                string message = puller.ReceiveFrameString();

                Console.WriteLine(message);
            }
        }

        static void Main(string[] args)
        {
            pusher = new PushSocket("ipc://NodeServer_1_1");

            //puller = new PullSocket();
            //puller.Bind("ipc://NodeServer_1_1");

            //mThread = new Thread(ReceivceLoop);
            //mThread.IsBackground = true;
            //mThread.Start();


            while (true)
            {
                pusher.TrySendFrame("test Inner");

                Thread.Sleep(100);
            }
        }

        static Thread mThread = null;

        static PushSocket pusher;

        static PullSocket puller;
    }
}
