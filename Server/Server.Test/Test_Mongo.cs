using Giant.DB;
using Giant.DB.MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Test
{
    class Test_Mongo
    {
        private Random random = new Random();

        public void Init()
        {
            DBService.Instance.Init(DataBaseType.MongoDB, "127.0.0.1:27017", "Giant", "dbOwner", "dbOwner");
        }

        public async void TestMongo()
        {
            //await TestInsertBatch();
            await TestFindBatch(10);
            await TestDelete();
            await TestFindBatch(10);
        }

        public async Task TestFindBatch(int num)
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

            var query = new MongoDBQueryBatch<Player>("Player", x => x.Uid > 1, options);
            var player = await query.Task();

            return player;
        }


        private Task<DeleteResult> TestDelete()
        {
            var task = new MongoDBDeleteBatch<Player>("Player", x => x.Uid > 0);
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

                var task = new MongoDBInsert<Player>("Player", player);
                await task.Task();
            }
        }

        private async Task TestInsertBatch()
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

            var task = new MongoDBInsertBatch<Player>("Player", player);
            await task.Task();
        }
    }
}
