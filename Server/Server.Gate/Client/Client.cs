using Giant.Core;
using Giant.Msg;
using Giant.Net;
using Giant.Share;
using System;

namespace Server.Gate
{
    public class Client : IDisposable
    {
        private AESCrypt AESCrypt;
        public string SecretKey => AESCrypt.EncryptKey;

        public Session Session { get; private set; }
        public string Account { get; set; }
        public int Uid { get; set; }

        public DateTime HeartBeatTime = TimeHelper.Now;

        public ZoneServer ZoneServer { get; private set; }

        public Client(Session session, string account)
        {
            Session = session;
            Account = account;
        }

        public void Offline()
        {
            ClientManager.Instance.Remove(Session.Id);
        }

        public void HeartBeat()
        {
            HeartBeatTime = TimeHelper.Now;
        }

        public void SetZoneServer(ZoneServer server)
        {
            ZoneServer = server;
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

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
