using Giant.DB;
using Giant.DB.MySQL;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Server.Test
{
    public class Test_MySql
    {
        private static int uid = 10000 * 10;
        private static  readonly Queue<MySqlInsertPlayerBatch> playerQueue = new Queue<MySqlInsertPlayerBatch>();

        public static void Init()
        {
            DataBaseService.Instance.Init(DataBaseType.MySQL, "39.97.243.209", "test", "wg", "giantwugui", 1);
        }

        public static void Test()
        {
            Init();

            GenerateData();

            GetMaxUid();

            Test_Insert();

            Test_Load();
        }

        public static void GenerateData()
        {
            PlayerInfo player;
            MySqlInsertPlayerBatch query;
            List<PlayerInfo> players = new List<PlayerInfo>();
            for (int i = 0; i < 500; ++i)
            {
                ++uid;
                player = new PlayerInfo()
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

                    players = new List<PlayerInfo>();
                }
            }
        }

        public static async void Test_Insert()
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

        public static async void Test_Load()
        {
            MySqlQueryPlayer query = new MySqlQueryPlayer(100001);
            PlayerInfo player = await query.Task();
        }

        public static async void GetMaxUid()
        {
            MySqlDeleteTable deleteTab = new MySqlDeleteTable("player");
            await deleteTab.Task();

            MySqlQueryMaxPlayerUid query = new MySqlQueryMaxPlayerUid();
            var max = await query.Task();
            uid = Math.Max(max, uid);
        }
    }
}
