using Giant.Net;
using Giant.Share;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.App
{
    public class Client
    {
        private readonly Session session;

        private AESCrypt AESCrypt;
        public string SecretKey => AESCrypt.EncryptKey;

        public string Account { get; private set; }
        public int Uid { get; private set; }

        public Client(string account, int uid, Session session)
        {
            Account = account;
            Uid = uid;
            this.session = session;
        }

        public void BuildSecretKey()
        {
            AESCrypt = new AESCrypt();
        }
    }
}
