using Giant.Log;
using Giant.Share;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Giant.Frame
{
    public class BaseService
    {
        delegate bool ControlCtrlHandle(int ctrlType);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ControlCtrlHandle HandlerRoutine, bool Add);
        private static readonly ControlCtrlHandle cancelHandler = new ControlCtrlHandle(HandleMathord);

        public static BaseService Instance { get; } = new BaseService();

        private BaseService()
        {
            // 异步方法全部会回掉到主线程
            SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);

            SetConsoleCtrlHandler(cancelHandler, true);
        }

        private static bool HandleMathord(int ctrlType)
        {
            switch (ctrlType)
            {
                case 0:
                    Console.WriteLine("0工具被强制关闭"); //Ctrl+C关闭
                    break;
                case 2:
                    Console.WriteLine("2工具被强制关闭");//按控制台关闭按钮关闭
                    break;
            }

            return true;
        }


        public void Start()
        {
            try
            {
                //框架的各种初始化工作
                ConsoleReader.Instance.Start(DoCmd);
                

                Console.WriteLine("server start finished ----------------------------------");
            }
            catch(Exception ex)
            {
                Logger.Error(ex);
            }

            while (true)
            {
                try
                {
                    OneThreadSynchronizationContext.Instance.Update();

                    Thread.Sleep(1);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
        }


        public virtual void DoCmd(string message)
        {
            switch (message)
            {
                default:
                    Logger.Info($"read message {message}");
                    break;
            }
        }


    }
}
