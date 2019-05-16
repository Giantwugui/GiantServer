using Giant.DB;
using Giant.DB.MongoDB;
using Giant.Frame;
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
            //所有异步回调到主线程
            SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);

            Test_Mongo mongo_test = new Test_Mongo();
            mongo_test.Init();
            mongo_test.TestMongo();

            //Test_MySql test_MySql = new Test_MySql();
            //test_MySql.Init();
            //test_MySql.Test();

            while (true)
            {
                Thread.Sleep(1);

                OneThreadSynchronizationContext.Instance.Update();

                //Console.ReadLine();

                //TestInsert();
            }
        }
       
    }
}
