using Giant.Core;
using Giant.Framework;
using Giant.Msg;

namespace Server.App
{
    public class UpLoadGateInfoComponent : InitSystem<FrontendComponent>
    {
        private long timerId;
        private FrontendComponent frontendComponent;

        public override void Init(FrontendComponent frontend)
        {
            frontendComponent = frontend;
            timerId = TimerComponent.Instance.AddRepeatTimer(3 * 1000, SendGateInfo).InstanceId;
        }

        public override void Dispose()
        {
            base.Dispose();
            TimerComponent.Instance.Remove(timerId);
        }

        private void SendGateInfo()
        {
            Msg_GateA_GateInfo msg = new Msg_GateA_GateInfo()
            {
                AppId = Scene.AppConfig.AppId,
                SubId = Scene.AppConfig.SubId,
                Address = Scene.AppConfig.OutterAddress,
                ClientCount = ClientManagerComponent.Instance.ClientCount()
            };
            frontendComponent.Session?.Notify(msg);
        }
    }
}
