using Giant.Net;

namespace Server.Gate
{
    partial class AppService
    {
        public override void OnConnect(Session session, bool isConnect)
        {
            if (isConnect)
            {
                ClientManager.Instance.Add2Watting(new Client(session));
            }
            else
            {
                Client client = ClientManager.Instance.GetClient(session.Id);
                client?.Offline();
            }
        }
    }
}
