using Giant.Core;
using System.Collections.Generic;
using System.Diagnostics;

namespace Robot
{
    public class ClientManagerComponent : InitSystem
    {
        private static string accountPre = "wuu";
        private Queue<Client> wattingLoginList = new Queue<Client>();

        public int TotalCount { get; set; }
        public Stopwatch Stopwatch { get; private set; }
        public static ClientManagerComponent Instance { get; private set; }
        public override void Init()
        {
            Instance = this;
        }

        public void CreateClients(int count)
        {
            TotalCount = count;
            for (int i = 0; i < count; ++i)
            {
                wattingLoginList.Enqueue(ComponentFactory.CreateComponentWithParent<Client, string>(this, accountPre + i));
            }

            Stopwatch = new Stopwatch();
            Stopwatch.Start();

            while (wattingLoginList.TryDequeue(out var player))
            {
                player.DoLogin();
            }
        }
    }
}
