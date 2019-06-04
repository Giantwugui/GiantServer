using Giant.Msg;
using Giant.Share;
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

        public override void SessionError(Session session, object error)
        {
        }

        public void HeartBeat(IMessage message)
        {
            innerSessions.ForEach(kv =>
            {
                if (!kv.Value.IsConnected)
                {
                    return;
                }

                kv.Value.Send(message);
            });
        }
    }
}
