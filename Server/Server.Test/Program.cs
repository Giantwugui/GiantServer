using Giant.DB;
using Giant.DB.MongoDB;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Test
{
    partial class Program
    {
        private static DBService dbService;

        static void Main(string[] args)
        {
            dbService = new DBService(DataBaseType.MongoDB);
            dbService.Start("127.0.0.1:27017", "Giant", "", "");

            //TestInsertBatch();
            TestMongo();

            while (true)
            {
                Thread.Sleep(1);

                Console.ReadLine();

                //TestInsert();
            }
        }
       
    }
}
