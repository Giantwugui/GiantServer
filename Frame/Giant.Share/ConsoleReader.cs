using System;
using System.Threading;
using System.Threading.Tasks;

namespace Giant.Share
{
    public class ConsoleReader
    {
        private Action<string> action;
        private readonly CancellationTokenSource cancellationTokenSource;

        public static ConsoleReader Instance { get; } = new ConsoleReader();

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
