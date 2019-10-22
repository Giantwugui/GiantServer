using Giant.Data;
using Giant.Msg;
using Giant.Share;
using Server.Frame;
using System;

namespace Server.Gate
{
    public class AccountServer : FrontendServer
    {
        private DateTime updateTime = TimeHelper.Now;
        public AccountServer(FrontendServerManager manager, AppConfig appConfig) : base(manager, appConfig)
        { }

        public override void Update()
        {
            base.Update();
            if (TimeHelper.Now > updateTime)
            {
                updateTime = TimeHelper.Now.AddSeconds(5);
                SendGateInfo();
            }
        }

        private void SendGateInfo()
        {
            Msg_GateA_GateInfo msg = new Msg_GateA_GateInfo()
            {
                AppId = Framework.AppId,
                SubId = Framework.SubId,
                IP = Session.RemoteIPEndPoint.Address.ToString(),
                Port = Session.RemoteIPEndPoint.Port,
                ClientCount = ClientManager.Instance.ClientCount()
            };
            Send(msg);
        }
    }
}
