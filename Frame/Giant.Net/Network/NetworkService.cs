using Giant.Log;
using Giant.Msg;
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
        WebSocket
    }

    public abstract class NetworkService : IDisposable
    {
        private BaseNetService service;
        private readonly NetworkType networkType;

        private readonly Dictionary<long, Session> sessions = new Dictionary<long, Session>();
        public Dictionary<long, Session> Sessions => sessions;

        public MessageDispatcher MessageDispatcher { get; set; }

        public IMessagePacker MessageParser { get; set; }

        public NetworkService(NetworkType network)
        {
            this.networkType = network;
            Init();
        }

        public NetworkService(NetworkType network, string address)
        {
            this.networkType = network;
            Init(address);
        }

        public Session GetSession(long id)
        {
            if (sessions.TryGetValue(id, out Session session))
            {
                return session;
            }

            return null;
        }

        public Session Create(string address)
        {
            return this.Create(NetworkHelper.ToIPEndPoint(address));
        }

        public Session Create(IPEndPoint endPoint)
        {
            BaseChannel channel = service.CreateChannel(endPoint);
            Session session = new Session(this, channel);
            session.Start();
            sessions.Add(session.Id, session);
            return session;
        }

        public void Remove(long id)
        {
            if (sessions.TryGetValue(id, out Session session))
            {
                session.Dispose();
                sessions.Remove(id);
            }

            int conns = sessions.Count;
            if (conns % 10 == 0)
            {
                Logger.Info($"conn num {conns}");
            }
        }

        public void Update()
        {
            service.Update();
        }

        private void Init(int packetSizeLength = Packet.PacketSizeLength2)
        {
            switch (this.networkType)
            {
                case NetworkType.Tcp:
                    service = new TcpService(packetSizeLength);
                    break;
                case NetworkType.Udp:
                    service = new UdpService();
                    break;
                case NetworkType.WebSocket:
                    service = new WebService();
                    break;
            }
        }

        private void Init(string address, int packetSizeLength = Packet.PacketSizeLength2)
        {
            IPEndPoint endPoint;
            switch (this.networkType)
            {
                case NetworkType.Tcp:
                    endPoint = NetworkHelper.ToIPEndPoint(address);
                    service = new TcpService(packetSizeLength, endPoint, OnAccept);
                    break;
                case NetworkType.Udp:
                    endPoint = NetworkHelper.ToIPEndPoint(address);
                    service = new UdpService(endPoint.Port, OnAccept);
                    break;
                case NetworkType.WebSocket:
                    service = new WebService(address.Split(";").ToList(), OnAccept);
                    break;
            }
        }

        private void OnAccept(BaseChannel baseChannel)
        {
            Session session = new Session(this, baseChannel);
            baseChannel.Start();

            sessions[session.Id] = session;

            int conns = sessions.Count;
            if (conns % 10 == 0)
            {
                Logger.Info($"conn nun {conns}");
            }
        }

        public void Dispose()
        {
        }
    }
}
