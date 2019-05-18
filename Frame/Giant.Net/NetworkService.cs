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

    public class NetworkService : IDisposable
    {
        private BaseService service;
        private readonly NetworkType networkType;

        private readonly Dictionary<long, Session> sessions = new Dictionary<long, Session>();
        public Dictionary<long, Session> Sessions => sessions;

        public MessageDispatcher MessageDispatcher { get; set; }

        public IMessagePacker MessageParser { get; set; }

        public NetworkService(NetworkType network)
        {
            this.networkType = network;
            this.MessageDispatcher = new MessageDispatcher();
            this.MessageParser = new ProtoPacker();

            Init();
        }

        public NetworkService(NetworkType network, string address)
        {
            this.networkType = network;
            this.MessageDispatcher = new MessageDispatcher();

            Init(address);
        }

        public Session GetSession(uint id)
        {
            if (sessions.TryGetValue(id, out Session session))
            {
                return session;
            }

            return null;
        }

        public Session Create(string address)
        {
            BaseChannel channel = service.CreateChannel(address);
            channel.Start();

            Session session = new Session(this, channel);

            sessions[session.Id] = session;

            return session;
        }

        public Session Create(IPEndPoint endPoint)
        {
            BaseChannel channel = service.CreateChannel(endPoint);
            channel.Start();

            Session session = new Session(this, channel);

            sessions[session.Id] = session;
            
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
                Logger.Info($"conn nun {conns}");
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
