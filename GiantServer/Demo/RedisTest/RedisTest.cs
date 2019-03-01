using Giant.Redis;
using Giant.Model;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Redis.HelperTest;

namespace Demo
{
    partial class Program
    {
        static int testNumber = 10000;


        static void TestRedis()
        {
            //var stringTest = new StringTest();
            //stringTest.TestStringAction();
            //stringTest.TestStringAsync();


            //KeyTest keyTest = new KeyTest();
            //keyTest.TestKeyAction();
            //keyTest.TestKeyAsync();

            //HashTest hashTest = new HashTest();
            //hashTest.TestHashAction();
            //hashTest.TestHashAsync();

            //ListTest listTest = new ListTest();
            //listTest.TestListAction();


            //SortedSetTest sortedSetTest = new SortedSetTest();
            //sortedSetTest.TestSortedSetAction();

            ReadWriteTest();

            //ListTest();


            //Player player = new Player() { Account = "Redis" };

            //RedisStringHelper.Instance.StringGet("int", "1");

            //int redisValue = RedisStringHelper.Instance.StringGet<int>("int");

            //RedisManager.Instance.HashSet("Player", player.Id.ToString(), player.ToJson());

            //Player cachePlayer = RedisManager.Instance.HashGet("Player", player.Id.ToString()).ToObject<Player>();
        }
        /// <summary>
        /// 读写测试
        /// </summary>
        static void ReadWriteTest()
        {
            string key = "TestTimeKey";
            var redis = RedisStringHelper.Instance;

            Stopwatch sw = new Stopwatch();

            {
                sw.Restart();
                for (int i = 0; i < testNumber; i++)
                {
                    redis.StringSet(i.ToString(), new Player { Account = $"wg{i}号" });
                    //redis.StringSet(key, "Giant");
                }
                sw.Stop();
                Console.WriteLine($"写{testNumber}次共耗时：{sw.ElapsedMilliseconds}毫秒");
            }

            {
                sw.Restart();
                for (int i = 0; i < testNumber; i++)
                {
                    redis.StringGet(i.ToString());
                }
                sw.Stop();
                Console.WriteLine($"读{testNumber}次共耗时：{sw.ElapsedMilliseconds}毫秒");
            }

            {
                sw.Restart();

                for (int i = 0; i < testNumber; i++)
                {
                    redis.StringSet(key, new Player { Account = $"wg{i}号" });
                    redis.StringGet(key);
                }
                sw.Stop();
                Console.WriteLine($"读写{testNumber}次共耗时：{sw.ElapsedMilliseconds}毫秒");
            }

            {
                sw.Restart();

                List<string> keys = new List<string>();
                for (int i = 0; i < testNumber; i++)
                {
                    redis.StringSet(key, new Player { Account = $"wg{i}号" });
                    keys.Add(i.ToString());
                }
                redis.StringGet(keys.ToArray());

                sw.Stop();
                Console.WriteLine($"读写{testNumber}次共耗时：{sw.ElapsedMilliseconds}毫秒");
            }
        }

        /// <summary>
        /// 测试Reids队列
        /// </summary>
        static void ListTest()
        {
            var redis = RedisListHelper.Instance;
            string key = "List_TestKey";

            redis.KeyFulsh();

            //开5个监控线程
            for (int i = 0; i < 10; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        var user = redis.ListRightPop<Player>(key);
                        if (null != user)
                        {
                            Console.WriteLine($"我是线程 [{Thread.CurrentThread.ManagedThreadId}], 处理了 [{user.Account}]");
                            //Thread.Sleep(1000);
                        }
                        //Thread.Sleep(1000);
                    }
                });
            }


            Console.Write("请输入测试人姓名：");
            while (true)
            {
                string wValue = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(wValue))
                {
                    for (int i = 0; i < 100; i++)
                    {
                        redis.ListLeftPush<Player>(key,
                            new Player {Account = $"{wValue}{i}号" });
                    }
                }
            }
        }
    }
}
