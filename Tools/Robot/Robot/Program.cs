using Giant.Core;
using Giant.Framework;
using Giant.Net;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Robot
{
    class Program
    {
        static CancellationTokenSource cancellationTokenSource;

        static void Main(string[] args)
        {
            try
            {
                Scene.EventSystem.Regist(Assembly.GetExecutingAssembly());

                Scene.Pool.AddComponent<TimerComponent>();
                Scene.Pool.AddComponent<OpcodeComponent>();
                Scene.Pool.AddComponent<MessageDispatcherComponent>();
                Scene.Pool.AddComponent<ClientManagerComponent>();
                Scene.Pool.AddComponent<PlayerManagerComponent>();
                Scene.Pool.AddComponent<OutterNetworkComponent, NetworkType>(NetworkType.Tcp);

                ClientManagerComponent.Instance.CreateClients(int.Parse(args[0]));
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
                    Scene.EventSystem.Update(0);
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
                default:
                    Console.WriteLine("Not suport cmd !");
                    break;
            }
        }

    }
}