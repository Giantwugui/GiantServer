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

        static void Main(string[] args)
        {
            //Test_Mongo mongo_test = new Test_Mongo();
            //mongo_test.TestMongo();

            Test_MySql test_MySql = new Test_MySql();
            test_MySql.Init();
            test_MySql.Test();

            while (true)
            {
                Thread.Sleep(1);

                Console.ReadLine();

                //TestInsert();
            }
        }
       
    }
}
