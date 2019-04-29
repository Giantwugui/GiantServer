using Giant.Log;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Giant.Frame
{
    public class BaseService
    {
        delegate bool ControlCtrlHandle(int ctrlType);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ControlCtrlHandle HandlerRoutine, bool Add);
        private static ControlCtrlHandle cancelHandler = new ControlCtrlHandle(HandleMathord);

        private static BaseService instance;
        public static BaseService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BaseService();
                }
                return instance;
            }
        }

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
                ReadLineAsync();

                Console.WriteLine("server start finished ----------------------------------");
            }
            catch
            {
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

        private async void ReadLineAsync()
        {
            //string 
            string content = await Task.Run(() =>
            {
                content = Console.In.ReadLine();//异步阻塞
                return content;
            });

            DoMessageDispatch(content);

            ReadLineAsync();
        }


        private void DoMessageDispatch(string message)
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
