using Giant.Msg;
using System.Collections.Generic;
using System.Net;

namespace Giant.Net
{
    public class InnerNetworkService : NetworkService
    {
        private readonly Dictionary<IPEndPoint, Session> innerSessions = new Dictionary<IPEndPoint, Session>();

        public InnerNetworkService(NetworkType network, string address) : base(network, address)
        {
            this.MessageParser = new ProtoPacker();
            this.MessageDispatcher = new MessageDispatcher();
        }

        public Session GetSession(IPEndPoint endPoint)
        {
            if (!innerSessions.TryGetValue(endPoint, out Session session))
            {
                session = base.Create(endPoint);
                innerSessions[endPoint] = session;
            }

            return session;
        }

    }
}
