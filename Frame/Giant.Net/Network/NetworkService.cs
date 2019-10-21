using System;
using System.Collections.Generic;
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

    public class NetworkService : IDisposable
    {
        private BaseNetService service;

        private readonly Dictionary<long, Session> sessions = new Dictionary<long, Session>();
        public Dictionary<long, Session> Sessions => sessions;

        public IMessagePacker MessageParser { get; set; }
        public MessageDispatcher MessageDispatcher { get; set; }


        public NetworkService(NetworkType network)
        {
            Init(network);
        }

        public NetworkService(NetworkType network, string address)
        {
            Init(network, address);
        }

        public Session GetSession(long id)
        {
            sessions.TryGetValue(id, out Session session);
            return session;
        }

        public Session Create(string address)
        {
            return this.Create(NetworkHelper.ToIPEndPoint(address));
        }

        public Session Create(IPEndPoint endPoint)
        {
            BaseChannel channel = service.CreateChannel(endPoint);
            Session session = new Session(this, channel);
            sessions.Add(session.Id, session);
            return session;
        }

        public virtual void Update()
        {
            service.Update();
        }

        public virtual void Remove(Session session)
        {
            sessions.Remove(session.Id);
        }

        public void Dispose()
        {
        }

        private void Init(NetworkType network)
        {
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

            this.MessageParser = new ProtoPacker();
            this.MessageDispatcher = new MessageDispatcher();
        }

        private void Init(NetworkType network, string address)
        {
            IPEndPoint endPoint;
            switch (network)
            {
                case NetworkType.Tcp:
                    endPoint = NetworkHelper.ToIPEndPoint(address);
                    service = new TcpService(Packet.PacketSizeLength2, endPoint, OnAccept);
                    break;
                case NetworkType.Udp:
                    endPoint = NetworkHelper.ToIPEndPoint(address);
                    service = new UdpService(endPoint.Port, OnAccept);
                    break;
                case NetworkType.Web:
                    service = new WebService(address.Split(";").ToList(), OnAccept);
                    break;
            }

            this.MessageParser = new ProtoPacker();
            this.MessageDispatcher = new MessageDispatcher();
        }

        private void OnAccept(BaseChannel baseChannel)
        {
            Session session = new Session(this, baseChannel);
            baseChannel.Start();

            sessions[session.Id] = session;
        }
    }
}
