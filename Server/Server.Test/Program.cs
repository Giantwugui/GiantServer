using System;
using System.Threading;
using Giant.DB;
using Giant.DB.MongoDB;
using MongoDB.Bson;
using System.Linq.Expressions;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Server.Test
{
    class Program
    {
        private static DBService dbService;

        static void Main(string[] args)
        {
            dbService = new DBService(DataBaseType.MongoDB);
            dbService.Start("127.0.0.1:27017", "Giant", "", "");

            FindPlayer();

            TestInsertBatch();

            while (true)
            {
                Thread.Sleep(1);

                Console.ReadLine();

                TestInsert();
            }
        }

        public static async void FindPlayer()
        {
            BsonDocument elements = new BsonDocument();
            elements.Add("Uid", 100010);

            var query = new MongoDBQueryBatch<Player>(dbService, "Player", x => x.Uid > 1);

            var player = await query.Task();
        }


        private static async void TestInsert()
        {
            long uid = 10000 * 10;
            Player player;
            for(int i = 0; i < 10; i++)
            {
                player = new Player
                {
                    Uid = ++uid,
                    Account = $"Account&{uid}"
                };

                MongoDBInsert task = new MongoDBInsert(dbService, player);

                await task.Task();
            }
        }

        private static async void TestInsertBatch()
        {
            long uid = 20000 * 10;
            List<Player> player = new List<Player>();
            for (int i = 0; i < 10; i++)
            {
                player.Add(new Player
                {
                    Uid = ++uid,
                    Account = $"Account&{uid}"
                });
            }

            var task = new MongoDBInsertBatch<Player>(dbService, player);

            await task.Task();
        }
    }
}
