using Giant.Core;
using Giant.Framework;
using Giant.Msg;
using Giant.Net;
using System;

namespace Server.Gate
{
    public class Client : Entity, IInitSystem<Session, string>
    {
        private AESCrypt AESCrypt;
        public string SecretKey => AESCrypt.EncryptKey;

        public Session Session { get; private set; }
        public string Account { get; set; }
        public int Uid { get; set; }

        public DateTime HeartBeatTime = TimeHelper.Now;

        public void Init(Session session, string account)
        {
            Session = session;
            Account = account;
        }

        public void Offline()
        {
            ClientManagerComponent.Instance.Remove(Session.Id);
        }

        public void HeartBeat()
        {
            HeartBeatTime = TimeHelper.Now;
        }

        public void EnterWorld()
        {
            Msg_GateZ_EnterWorld msg = new Msg_GateZ_EnterWorld { Uid = Uid };
            SendToZone(msg);
        }

        public void SendToClient(IMessage message)
        {
            Session.Notify(message);
        }

        public void SendToZone(IMessage message)
        {
            GetComponent<FrontendComponent>()?.Send(message);
        }

        public void BuildSecretKey()
        {
            AESCrypt = new AESCrypt();
        }
    }
}
