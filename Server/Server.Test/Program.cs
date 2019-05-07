using System;
using System.Threading;
using Giant.DB;
using Giant.DB.MongoDB;

namespace Server.Test
{
    class Program
    {
        private static DBService dbService;

        static void Main(string[] args)
        {
            dbService = new DBService(DataBaseType.MongoDB);
            dbService.Start("127.0.0.1:27017", "Giant", "", "");


            while (true)
            {
                Thread.Sleep(1);

                Console.ReadLine();

                TestInsert();
            }
        }


        private static void TestInsert()
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

                MongoDBSaveTask task = new MongoDBSaveTask(dbService, player);
                InsertToDB(task);
            }
        }

        private static async void InsertToDB(MongoDBSaveTask task)
        {
            await task.Task();
        }
    }
}
