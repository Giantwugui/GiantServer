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

        public IMessagePacker MessageParser { get; set; }

        public Session Create(string address)
        {
            return Create(NetworkHelper.ToIPEndPoint(address));
        }

        public Session Create(IPEndPoint endPoint)
        {
            BaseChannel channel = service.CreateChannel(endPoint);

            Session session = ComponentFactory.CreateComponentWithParent<Session, BaseChannel>(this, channel);
            AddChild(session);
            return session;
        }

        public virtual void Update()
        {
            service.Update();
        }

        public virtual void Remove(Session session)
        {
            RemoveChild(session.InstanceId);
            OnDisconnecte(session);
        }

        public void Init(NetworkType network)
        {
            MessageParser = new ProtoPacker();
            switch (network)
            {
                case NetworkType.Tcp:
                    service = new TcpService(Packet.PacketSizeLength2);
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
                    service = new TcpService(Packet.PacketSizeLength2, endPoint, channel => OnAccept(channel));
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

        public virtual Session OnAccept(BaseChannel baseChannel)
        {
            Session session = ComponentFactory.CreateComponentWithParent<Session, BaseChannel>(this, baseChannel);
            baseChannel.Start();

            AddChild(session);
            return session;
        }

        public virtual void OnDisconnecte(Session session)
        {
        }
    }
}
