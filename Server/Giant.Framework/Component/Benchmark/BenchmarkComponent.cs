using Giant.Core;
using Giant.Logger;
using Giant.Msg;
using Giant.Net;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace Giant.Framework
{
    public class BenchmarkComponent : InitSystem
    {
        private int num = 0;
        private Stopwatch stopwatch;

        public override void Init()
        {
            stopwatch = new Stopwatch();
        }

        public void Test()
        {
            IPEndPoint ipEndPoint = NetworkHelper.ToIPEndPoint("127.0.0.1:9001");
            TestAsync(ipEndPoint);
        }

        public void TestAsync(IPEndPoint ipEndPoint)
        {
            OutterNetworkComponent component = Scene.Pool.GetComponent<OutterNetworkComponent>();
            Session session = component.Create(ipEndPoint);
            session.OnConnectCallback += (async (se, state) =>
            {
                if (state)
                {
                    stopwatch.Start();
                    int i = 0;
                    int count = 50 * 10000;
                    while (i++ < count)
                    {
                        await Send(session);
                    }
                    stopwatch.Stop();
                }
            });
            session.Start();
        }

        public async Task Send(Session session)
        {
            try
            {
                await session.Call(new Msg_CG_HeartBeat_Ping());
                ++num;

                if (num % 100000 != 0)
                {
                    return;
                }
                Log.Warn($"Benchmark k: {num} 每10W次耗时: {stopwatch.ElapsedMilliseconds} ms {session.GetParent<NetworkComponent>()?.Children.Count}");
                stopwatch.Restart();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}
