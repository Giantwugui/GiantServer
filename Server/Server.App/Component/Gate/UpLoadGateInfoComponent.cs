using Giant.Core;
using Giant.Framework;
using Giant.Msg;
using System;

namespace Server.App
{
    public class UpLoadGateInfoComponent : InitSystem<FrontendComponent>, IUpdateSystem
    {
        private DateTime updateTime = TimeHelper.Now;
        private FrontendComponent frontendComponent;

        public void Update(double dt)
        {
            if (TimeHelper.Now > updateTime)
            {
                updateTime = TimeHelper.Now.AddSeconds(5);
                SendGateInfo();
            }
        }

        public override void Init(FrontendComponent frontend)
        {
            frontendComponent = frontend;
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
