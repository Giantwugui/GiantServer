using Giant.DB;
using Giant.DB.MySQL;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Server.Test
{
    public class Test_MySql
    {
        private int uid = 10000 * 10;
        private readonly Queue<MySqlInsertPlayerBatch> playerQueue = new Queue<MySqlInsertPlayerBatch>();

        public void Init()
        {
            DataBaseService.Instance.Init(DataBaseType.MySQL, "127.0.0.1", "wg_test", "root", "root", 1);
        }

        public void Test()
        {
            GenerateData();

            GetMaxUid();

            Test_Insert();

            Test_Load();
        }

        public void GenerateData()
        {
            Player player;
            MySqlInsertPlayerBatch query;
            List<Player> players = new List<Player>();
            for (int i = 0; i < 500; ++i)
            {
                ++uid;
                player = new Player()
                {
                    Account = $"wg_{uid}",
                    Level = 10,
                    Uid = uid,
                };

                players.Add(player);

                if (i % 20 == 0)
                {
                    query = new MySqlInsertPlayerBatch(players);
                    playerQueue.Enqueue(query);

                    players = new List<Player>();
                }
            }
        }

        public async void Test_Insert()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            while (playerQueue.TryDequeue(out var task))
            {
                int insertCount = await task.Task();
            }

            watch.Stop();
            Console.WriteLine($"cost time {watch.ElapsedMilliseconds}");
        }

        public async void Test_Load()
        {
            MySqlQueryPlayer query = new MySqlQueryPlayer(10001);
            Player player = await query.Task();
        }

        public async void GetMaxUid()
        {
            MySqlDeleteTable deleteTab = new MySqlDeleteTable("Player");
            await deleteTab.Task();

            MySqlQueryMaxPlayerUid query = new MySqlQueryMaxPlayerUid();
            var max = await query.Task();
            this.uid = Math.Max(max, uid);
        }
    }
}
