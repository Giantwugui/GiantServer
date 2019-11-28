using Giant.Core;
using Giant.Net;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static CancellationTokenSource cancellationTokenSource;

        static void Main(string[] args)
        {
            try
            {
                //Scene.Pool.AddComponentWithCreate<DataComponent>();
                Scene.Pool.AddComponentWithCreate<OpcodeComponent>();
                Scene.Pool.AddComponentWithCreate<MessageDispatcherComponent>();
                Scene.Pool.AddComponentWithCreate<OutterNetworkComponent, NetworkType>(NetworkType.Tcp);

                Scene.EventSystem.Add(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            while (true)
            {
                try
                {
                    ReadLineAsync();

                    OneThreadSynchronizationContext.Instance.Update();
                    PlayerManager.Instance.Update();
                    Thread.Sleep(1);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

        }

        private static async void ReadLineAsync()
        {
            cancellationTokenSource = new CancellationTokenSource();
            while (true)
            {
                string inStr = await Task.Run(() => Console.In.ReadLineAsync(), cancellationTokenSource.Token);

                DoCmd(inStr);
            }
        }

        private static void DoCmd(string content)
        {
            string[] param = content.Split(' ');
            if (param.Length <= 0)
            {
                Console.WriteLine("Param error !");
            }

            switch (param[0])
            {
                case "Login":
                    {
                        NET.DoLogin(param[1], "");
                    }
                    break;
                default:
                    Console.WriteLine("Not suport cmd !");
                    break;
            }
        }

    }
}