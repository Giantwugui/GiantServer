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

    public class NetworkService : IUpdate,IDisposable
    {
        private BaseService service;

        private NetworkType networkType;

        private Dictionary<uint, Session> sessions = new Dictionary<uint, Session>();

        public Dictionary<uint, Session> Sessions => sessions;


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

        public void Remove(uint id)
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
