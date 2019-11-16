using Giant.Core;
using Giant.Net;
using Giant.Share;
using System;

namespace Server.Account
{
    public class Client
    {
        private AESCrypt AESCrypt;
        public string SecretKey => AESCrypt.EncryptKey;

        public Session Session { get; private set; }
        public string Account { get; private set; }

        public DateTime HeartBeatTime = TimeHelper.Now;

        public Client(Session session)
        {
            Session = session;
        }

        public void Offline()
        {
            ClientManager.Instance.Remove(Session.Id);
        }

        public void HeartBeat()
        {
            HeartBeatTime = TimeHelper.Now;
        }
    }
}
