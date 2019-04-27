using System;
using System.Collections.Generic;
using System.Net;
using Giant.Share;

namespace Giant.Model
{
    public enum NetworkType
    {
        Tcp,
        Udp,
        WebSocket
    }

    class NetworkComponent : Component
    {
        private BaseService service;

        private NetworkType networkType;

        private Dictionary<long, Session> sessions = new Dictionary<long, Session>();

        public Dictionary<long, Session> Sessions => sessions;


        public void Awake(NetworkType network)
        {
            this.networkType = network;

            Init();
        }

        public void Awake(NetworkType network, string address)
        {
            this.networkType = network;

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

        public Session CreateSession(string address)
        {
            BaseChannel channel = service.CreateChannel(address);
            channel.Start();

            Session session = new Session(this, channel);

            sessions[session.Id] = session;

            return session;
        }

        public Session CreateSession(IPEndPoint endPoint)
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
                Console.WriteLine($"conn nun {conns}");
            }
        }

        public void Update()
        {
            service.Update();
        }

        private void Init()
        {
            switch (this.networkType)
            {
                case NetworkType.Tcp:
                    service = new TcpService();
                    break;
                case NetworkType.Udp:
                    service = new UdpService();
                    break;
                case NetworkType.WebSocket:
                    service = new HttpService();
                    break;
            }
        }

        private void Init(string address)
        {
            IPEndPoint endPoint;
            switch (this.networkType)
            {
                case NetworkType.Tcp:
                    endPoint = NetworkHelper.ToIPEndPoint(address);
                    service = new TcpService(endPoint, OnAccept);
                    break;
                case NetworkType.Udp:
                    endPoint = NetworkHelper.ToIPEndPoint(address);
                    service = new UdpService(endPoint.Port, OnAccept);
                    break;
                case NetworkType.WebSocket:
                    service = new HttpService(address.Split(";").ToList(), OnAccept);
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
                Console.WriteLine($"conn nun {conns}");
            }
        }

        public void Dispose()
        {
        }
    }
}
