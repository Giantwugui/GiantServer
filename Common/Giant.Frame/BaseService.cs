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
        private CancellationTokenSource CancellationTokenSource;

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
            CancellationTokenSource = new CancellationTokenSource();

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
            while (true)
            {
                try
                {
                    string line = await Task.Run(() =>
                    {
                        return Console.In.ReadLine();
                    }, CancellationTokenSource.Token);

                    DoMessageDispatch(line);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
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
