using Giant.Core;
using Giant.Msg;
using Giant.Net;
using System;

namespace Server.App
{
    public class UpLoadGateInfoComponent :  Component, IUpdateSystem, IInitSystem<Session>
    {
        private DateTime updateTime = TimeHelper.Now;
        private Session session;

        public void Update(double dt)
        {
            if (TimeHelper.Now > updateTime)
            {
                updateTime = TimeHelper.Now.AddSeconds(5);
                SendGateInfo();
            }
        }

        public void Init(Session session)
        {
            this.session = session;
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
            session?.Notify(msg);
        }
    }
}
