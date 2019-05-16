using Giant.DB;
using Giant.DB.MySQL;
using System;
using System.Collections.Generic;

namespace Server.Test
{
    public class Test_MySql
    {
        private int uid = 10000 * 10;
        private DBService dbService;

        public void Init()
        {
            dbService = new DBService(DataBaseType.MySQL);
            dbService.Start("127.0.0.1", "wg_test", "root", "root");
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
            MySqlInsertPlayerBatch query = new MySqlInsertPlayerBatch(this.dbService, players);
            var result = await query.Task();
        }

        public async void Test_Load()
        {
            MySqlQueryPlayer query = new MySqlQueryPlayer(dbService, 10001);
            Player player = await query.Task();
        }

        public async void GetMaxUid()
        {
            MySqlQueryMaxPlayerUid query = new MySqlQueryMaxPlayerUid(this.dbService);

            var max = await query.Task();

            this.uid = Math.Max(max, uid);
        }
    }
}
