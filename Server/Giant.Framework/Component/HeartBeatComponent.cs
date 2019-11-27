using Giant.Core;
using Giant.Logger;
using Giant.Msg;
using Giant.Net;
using System;
using System.Threading;

namespace Giant.Framework
{
    class HeartBeatComponent : Component, IInitSystem<Session, int>, IUpdateSystem
    {
        private Session session;
        private int delayTime = 30;
        private CancellationTokenSource cancellation;
        private DateTime lastHeatBeatTime = TimeHelper.Now;

        public bool IsConnected => session != null && session.IsConnected;

        public void Init(Session session, int delayTime)
        {
            this.session = session;
            this.delayTime = delayTime;
        }

        public void Update(double dt)
        {
            CheckHeartBeat();
        }

        private void CheckHeartBeat()
        {
            if (!IsConnected)
            {
                return;
            }

            if ((TimeHelper.Now - lastHeatBeatTime).TotalSeconds < delayTime)
            {
                return;
            }

            HeartBeat();
            lastHeatBeatTime = TimeHelper.Now;
        }

        private async void HeartBeat()
        {
            Msg_HeartBeat_Ping ping = new Msg_HeartBeat_Ping
            {
                AppType = (int)Scene.AppConfig.AppType,
                AppId = Scene.AppConfig.AppId,
                SubId = Scene.AppConfig.SubId,
            };

            cancellation?.Cancel();
            cancellation = new CancellationTokenSource(delayTime * 1000);

            if (await session.Call(ping, cancellation.Token) is Msg_HeartBeat_Pong message)
            {
                Log.Info($"heart beat pong from appType {(AppType)message.AppType} appId {message.AppId} subId {message.SubId}");
            }

            cancellation.Dispose();
            cancellation = null;
        }
    }
}
