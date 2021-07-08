using System.Collections.Generic;
using Giant.Core;
using System.Linq;
using System.Net;

namespace Giant.Net
{
    public enum NetworkType
    {
        Tcp,
        Udp,
        Web
    }

    public abstract class NetworkComponent : Entity, IInitSystem<NetworkType>, IInitSystem<NetworkType, string>
    {
        private BaseNetService service;

        public readonly Dictionary<long, Session> Sessions = new Dictionary<long, Session>();

        public IMessagePacker MessageParser { get; set; }

        public void Init(NetworkType network)
        {
            MessageParser = new ProtoPacker();
            switch (network)
            {
                case NetworkType.Tcp:
                    service = new TcpService();
                    break;
                case NetworkType.Udp:
                    service = new UdpService();
                    break;
                case NetworkType.Web:
                    service = new WebService();
                    break;
            }
        }

        public void Init(NetworkType network, string address)
        {
            MessageParser = new ProtoPacker();
            IPEndPoint endPoint;
            switch (network)
            {
                case NetworkType.Tcp:
                    endPoint = NetworkHelper.ToIPEndPoint(address);
                    service = new TcpService(endPoint, channel => OnAccept(channel));
                    break;
                case NetworkType.Udp:
                    endPoint = NetworkHelper.ToIPEndPoint(address);
                    service = new UdpService(endPoint.Port, channel => OnAccept(channel));
                    break;
                case NetworkType.Web:
                    service = new WebService(address.Split(";").ToList(), channel => OnAccept(channel));
                    break;
            }
        }

        protected virtual Session OnAccept(BaseChannel channel)
        {
            Session session = ComponentFactory.CreateWithParent<Session, BaseChannel>(this, channel);
			Sessions.Add(session.Id, session);
            channel.Start();
            return session;
        }

        protected virtual void OnDisconnect(Session session)
        {
        }

        public virtual void Remove(long id)
        {
            if (!this.Sessions.TryGetValue(id, out var session))
            {
                return;
            }
            this.Sessions.Remove(id);
            session.Dispose();
        }

        public Session Get(long id)
        {
            this.Sessions.TryGetValue(id, out var session);
            return session;
        }

        public Session Create(string address)
        {
            return Create(NetworkHelper.ToIPEndPoint(address));
        }

        public Session Create(IPEndPoint endPoint)
        {
            BaseChannel channel = service.CreateChannel(endPoint);
            Session session = ComponentFactory.CreateWithParent<Session, BaseChannel>(this, channel);
			Sessions.Add(session.Id, session);
            channel.Start();
            return session;
        }

        public virtual void Update()
        {
            service.Update();
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            service.Dispose();
        }
    }
}
