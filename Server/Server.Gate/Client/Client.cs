using Giant.Core;
using Giant.Msg;
using Giant.Net;
using Giant.Share;
using System;

namespace Server.Gate
{
    public class Client
    {
        private AESCrypt AESCrypt;
        public string SecretKey => AESCrypt.EncryptKey;

        public Session Session { get; private set; }
        public string Account { get; private set; }
        public int Uid { get; private set; }

        public DateTime HeartBeatTime = TimeHelper.Now;

        public ZoneServer ZoneServer { get; private set; }

        public Client(Session session)
        {
            this.Session = session;
        }

        public void Online(string account, int uid)
        {
            Uid = uid;
            Account = account;
        }

        public void Offline()
        {
            ClientManager.Instance.Remove(this.Session.Id);
        }

        public void HeartBeat()
        {
            HeartBeatTime = TimeHelper.Now;
        }

        public void SetZoneServer(ZoneServer server)
        {
            this.ZoneServer = server;
        }

        public void EnterWorld()
        { 
        }

        public void SendToClient(IMessage message)
        {
            Session.Notify(message);
        }

        public void SendToZone(IMessage message)
        {
            ZoneServer.Send(message);
        }

        public void BuildSecretKey()
        {
            AESCrypt = new AESCrypt();
        }
    }
}
