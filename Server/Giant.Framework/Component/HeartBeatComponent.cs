using Giant.Core;
using Giant.Logger;
using Giant.Msg;
using Giant.Net;
using System.Threading;

namespace Giant.Framework
{
    class HeartBeatComponent : InitSystem<Session, int>
    {
        private long timerId;
        private Session session;
        private CancellationTokenSource cancellation;

        public bool IsConnected => session != null && session.IsConnected;

        public override void Init(Session session, int delayTime)
        {
            this.session = session;
            timerId = TimerComponent.Instance.AddRepeatTimer(10 * 1000, HeartBeat).InstanceId;
        }

        public override void Dispose()
        {
            base.Dispose();
            TimerComponent.Instance.Remove(timerId);
        }

        private async void HeartBeat()
        {
            if (!IsConnected) return;

            Msg_HeartBeat_Ping ping = new Msg_HeartBeat_Ping
            {
                AppType = (int)Scene.AppConfig.AppType,
                AppId = Scene.AppConfig.AppId,
                SubId = Scene.AppConfig.SubId,
            };

            cancellation?.Cancel();
            cancellation = new CancellationTokenSource(1 * 1000);

            if (await session.Call(ping, cancellation.Token) is Msg_HeartBeat_Pong message)
            {
                Log.Info($"heart beat pong from appType {(AppType)message.AppType} appId {message.AppId} subId {message.SubId}");
            }

            cancellation.Dispose();
            cancellation = null;
        }
    }
}
