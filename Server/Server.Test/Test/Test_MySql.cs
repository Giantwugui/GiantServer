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
            DataBaseService.Instance.Init(DataBaseType.MySQL, "127.0.0.1", "wg_test", "root", "root", 1);
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
            MySqlInsertPlayerBatch query = null;
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

                if (i % 20 == 0 && i != 0)
                {
                    query = new MySqlInsertPlayerBatch(players);
                    playerQueue.Enqueue(query);

                    players = new List<PlayerInfo>();
                }
            }
            playerQueue.Enqueue(query);
        }

        public static async void Test_Insert()
        {
            //MySqlInsertPlayer insertPlayer = new MySqlInsertPlayer(new PlayerInfo() { Uid = 111, Account = "111", Level = 0 });
            //await insertPlayer.Run();

            Stopwatch watch = new Stopwatch();
            watch.Start();

            int insertCount = 0;
            while (playerQueue.TryDequeue(out var task))
            {
                insertCount += await task.Task();
            }

            watch.Stop();
            Console.WriteLine($"cost time {watch.ElapsedMilliseconds} insert count {insertCount}");
        }

        public static async void Test_Load()
        {
            MySqlQueryPlayer query = new MySqlQueryPlayer(100001);
            PlayerInfo player = await query.Task();
            Console.WriteLine($"player info {player.Account} {player.Uid}");
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
