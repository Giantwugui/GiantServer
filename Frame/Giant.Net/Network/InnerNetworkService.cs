using Giant.Msg;
using Giant.Share;
using System.Collections.Generic;
using System.Net;

namespace Giant.Net
{
    public class InnerNetworkService : NetworkService
    {
        private readonly Dictionary<string, Session> innerSessions = new Dictionary<string, Session>();

        public InnerNetworkService(NetworkType network, string address) : base(network, address)
        {
            this.MessageParser = new ProtoPacker();
            this.MessageDispatcher = new MessageDispatcher();
        }

        public Session GetSession(string address)
        {
            if (!innerSessions.TryGetValue(address, out Session session))
            {
                session = base.Create(address);
                innerSessions[address] = session;
            }

            return session;
        }
    }
}
