using System.Collections.Generic;

namespace Giant.Net
{
    public class InnerNetComponent : NetworkComponent
    {
        private readonly Dictionary<string, Session> innerSessions = new();

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
    }
}
