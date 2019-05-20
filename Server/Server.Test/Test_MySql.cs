using Giant.DB;
using Giant.DB.MySQL;
using System;
using System.Collections.Generic;

namespace Server.Test
{
    public class Test_MySql
    {
        private int uid = 10000 * 10;

        public void Init()
        {
            DBService.Instance.Init(DataBaseType.MySQL, "127.0.0.1", "wg_test", "root", "root");
        }

        public void Test()
        {
            GetMaxUid();

            Test_Insert();
            Test_Load();
        }

        public async void Test_Insert()
        {
            Player player;
            List<Player> players = new List<Player>();
            for (int i = 0; i < 5; ++i)
            {
                ++uid;
                player = new Player()
                {
                    Account = $"wg_{uid}",
                    Level = 10,
                    Uid = uid,
                };

                players.Add(player);
            }
            MySqlInsertPlayerBatch query = new MySqlInsertPlayerBatch(players);
            var result = await query.Task();
        }

        public async void Test_Load()
        {
            MySqlQueryPlayer query = new MySqlQueryPlayer(10001);
            Player player = await query.Task();
        }

        public async void GetMaxUid()
        {
            MySqlQueryMaxPlayerUid query = new MySqlQueryMaxPlayerUid();

            var max = await query.Task();

            this.uid = Math.Max(max, uid);
        }
    }
}
