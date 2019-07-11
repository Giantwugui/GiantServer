using System.Collections.Generic;

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
                session = Create(address);
                innerSessions[address] = session;
            }

            return session;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Remove(Session session)
        {
        }
    }
}
