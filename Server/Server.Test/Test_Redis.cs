using System;
using System.Collections.Generic;
using System.Text;
using Giant.Redis;

namespace Server.Test
{
    public class Test_Redis
    {
        public static Test_Redis Instance { get; } = new Test_Redis();

        private Test_Redis()
        {
        }

        public void Init()
        {
            RedisService.Instance.Init("127.0.0.1:6379", "", 0);

            TestInsert();
        }

        public async void TestInsert()
        {
            PlayerInfo info;
            for (int i = 0; i < 5; ++i)
            {
                info = new PlayerInfo()
                {
                    Uid = i,
                    Account = DateTime.Now.ToString(),
                    Level = i,
                    Year = i * 10
                };

                RedisSavePlayer opera = new RedisSavePlayer(info);
                await opera.Task();
            }
        }
    }
}
