using Giant.Core;
using Giant.Logger;
using Giant.Msg;
using Giant.Net;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Server.Map
{
    public class BenchmarkComponent : Component
    {
        static int num = 0;
        static long time1 = 0;

        public static void Test()
        {
            try
            {
                time1 = TimeHelper.NowMilliSeconds;
                IPEndPoint ipEndPoint = NetworkHelper.ToIPEndPoint("127.0.0.1:8001");
                for (int i = 0; i < 2000; i++)
                {
                    TestAsync(ipEndPoint, i);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static async void TestAsync(IPEndPoint ipEndPoint, int j)
        {
            try
            {
                OutterNetworkComponent component = Scene.Pool.GetComponent<OutterNetworkComponent>();
                using (Session session = component.Create(ipEndPoint))
                {
                    session.Start();

                    int i = 0;
                    while (i < 100000000)
                    {
                        ++i;
                        await Send(session, j);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static async Task Send(Session session, int j)
        {
            try
            {
                await session.Call(new Msg_CG_HeartBeat_Ping());
                ++num;

                if (num % 100000 != 0)
                {
                    return;
                }

                long time2 = TimeHelper.NowMilliSeconds;
                long time = time2 - time1;
                time1 = time2;
                Log.Warn($"Benchmark k: {num} 每10W次耗时: {time} ms {session.NetworkService.Sessions.Count}");
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}
