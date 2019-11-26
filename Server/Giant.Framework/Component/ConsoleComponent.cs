using Giant.Core;
using Giant.Logger;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Giant.Framework
{
    public class ConsoleComponent : Component, IInitSystem
    {
        private CancellationTokenSource cancellationTokenSource;

        public static ConsoleComponent Instance { get; private set; }

        public ConsoleComponent()
        {
        }

        public void Init()
        {
            Instance = this;
            cancellationTokenSource = new CancellationTokenSource();

            ReadLineAsync();
        }

        private async void ReadLineAsync()
        {
            while (true)
            {
                string inStr = await Task.Run(() => Console.In.ReadLineAsync(), cancellationTokenSource.Token);

                ConsoleRead(inStr);
            }
        }

        private void ConsoleRead(string content)
        {
            switch (content)
            {
                case "load":
                    Scene.EventSystem.Load();
                    break;
                default:
                    Log.Warn("not support commond !");
                    break;
            }
        }
    }
}
