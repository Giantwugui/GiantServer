using Giant.Core;
using Giant.EnumUtil;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Giant.Framework
{
    public class ConsoleComponent : InitSystem
    {
        private CancellationTokenSource cancellationTokenSource;

        public static ConsoleComponent Instance { get; private set; }

        public override void Init()
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

                Scene.EventSystem.Handle(EventType.CommandLine, inStr);
            }
        }
    }
}
