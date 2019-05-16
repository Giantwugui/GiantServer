using Giant.DB;
using Giant.DB.MongoDB;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Server.Test
{
    class Test_Mongo
    {
        private DBService dbService;
        private Random random = new Random();

        public void Init()
        {
            dbService = new DBService(DataBaseType.MongoDB);
            dbService.Start("127.0.0.1:27017", "Giant", "", "");
        }

        public async void TestMongo()
        {
            TestFindBatch(10);
            await TestDelete();
            TestFindBatch(10);
        }

        public async void TestFindBatch(int num)
        {
            List<Player> players = await FindPlayerBySort(num);

            Console.WriteLine($"player count {players.Count}");
        }

        public async Task<List<Player>> FindPlayerBySort(int limit = 99999)
        {
            BsonDocument bsons = new BsonDocument()
            {
                new BsonElement("Level", 1),
            };

            FindOptions<Player> options = new FindOptions<Player>
            {
                Limit = limit,
                Sort = bsons
            };

            var query = new MongoDBQueryBatch<Player>(dbService, "Player", x => x.Uid > 1, options);
            var player = await query.Task();

            return player;
        }


        private Task<DeleteResult> TestDelete()
        {
            var task = new MongoDBDeleteBatch<Player>(dbService, "Player", x => x.Uid > 0);
            return task.Task();
        }

        private async void TestInsert()
        {
            long uid = 10000 * 10;
            Player player;
            for (int i = 0; i < 10; i++)
            {
                player = new Player
                {
                    Uid = ++uid,
                    Account = $"Account&{uid}"
                };

                var task = new MongoDBInsert<Player>(dbService, "Player", player);
                await task.Task();
            }
        }

        private async void TestInsertBatch()
        {
            long uid = 20000 * 10;
            List<Player> player = new List<Player>();
            for (int i = 0; i < 20; i++)
            {
                player.Add(new Player
                {
                    Uid = ++uid,
                    Account = $"Account&{uid}",
                    Level = random.Next(10, 100)
                });
            }

            var task = new MongoDBInsertBatch<Player>(dbService, "Player", player);
            await task.Task();
        }
    }
}
