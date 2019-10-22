using Giant.Core;
using Giant.Msg;
using Giant.Net;

namespace Server.Gate
{
    public class Client
    {
        private readonly Session session;

        private AESCrypt AESCrypt;
        public string SecretKey => AESCrypt.EncryptKey;

        public string Account { get; private set; }
        public int Uid { get; private set; }

        public ZoneServer ZoneServer { get; private set; }

        public Client(string account, int uid, Session session)
        {
            Account = account;
            Uid = uid;
            this.session = session;
        }

        public void SetZoneServer(ZoneServer server)
        {
            this.ZoneServer = server;
        }

        public void SendToClient(IMessage message)
        {
            session.Notify(message);
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
