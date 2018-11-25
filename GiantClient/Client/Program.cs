using System;
using System.Text;
using System.Threading;
using GiantCore;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                NetServices.ToStart();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            while (true)
            {
                string cmd = Console.ReadLine();

                if (cmd == "Test")
                {
                    NetServices.Send(new OuterMessage() {  ToNode = 1, Content = Encoding.UTF8.GetBytes("client") });
                    //ThreadHelper.CreateThread(ThreadLoop, "Send");
                }
            }
        }

        private static void ThreadLoop()
        {
            while (true)
            {

                NetServices.Send(new OuterMessage() {  ToNode = 1, Content = Encoding.UTF8.GetBytes("client") });
                Thread.Sleep(100);
            }
        }


    }
}
