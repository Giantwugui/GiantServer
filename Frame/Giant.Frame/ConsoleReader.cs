using System;
using System.Threading;
using System.Threading.Tasks;

namespace Giant.Frame
{
    public class ConsoleReader
    {
        private Action<string> action;
        private readonly CancellationTokenSource cancellationTokenSource;

        private static ConsoleReader instance;
        public static ConsoleReader Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConsoleReader();
                }
                return instance;
            }
        }

        private ConsoleReader()
        {
            this.cancellationTokenSource = new CancellationTokenSource();
        }

        public void Start(Action<string> callback)
        {
            this.action = callback;
            this.ReadLineAsync();
        }

        private async void ReadLineAsync()
        {
            while (true)
            {
                string inStr = await Task.Run(() => Console.In.ReadLineAsync(), this.cancellationTokenSource.Token);

                this.action?.Invoke(inStr);
            }
        }
    }
}
