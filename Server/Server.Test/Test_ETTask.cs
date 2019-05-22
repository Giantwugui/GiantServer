using ETModel;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Test
{
    partial class Program
    {
        public static void Start()
        {
            TestVoid().Coroutine();
        }


        public static async ETVoid TestVoid()
        {
            bool awaitValue = await GetBoolValue();

            Console.WriteLine($"Await value {awaitValue}");
        }

        public static ETTask<bool> GetBoolValue()
        {
            ETTaskCompletionSource<bool> completionSource = new ETTaskCompletionSource<bool>();

            SetResult(completionSource);

            return completionSource.Task;
        }

        public static async void SetResult(ETTaskCompletionSource<bool> completionSource)
        {
            await Task.Delay(50000);
            completionSource.SetResult(true);
        }



        public class ConsoleComponent
        {
            public CancellationTokenSource CancellationTokenSource;
            public string Mode = "";

            public async ETVoid Start()
            {
                this.CancellationTokenSource = new CancellationTokenSource();

                while (true)
                {
                    try
                    {
                        string line = await Task.Run(() =>
                        {
                            Console.Write($"{this.Mode}> ");
                            return Console.In.ReadLine();
                        }, this.CancellationTokenSource.Token);

                        line = line.Trim();

                        if (this.Mode != "")
                        {
                            bool isExited = true;
                            switch (this.Mode)
                            {

                            }

                            if (isExited)
                            {
                                this.Mode = "";
                            }

                            continue;
                        }

                        switch (line)
                        {

                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }
    }
}
