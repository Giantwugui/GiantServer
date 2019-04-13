using Giant.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Giant.Net
{
    public enum NetworkType
    {
        Tcp,
        Udp,
    }

    public class NetworkService : IUpdate,IDisposable
    {
        private BaseService service;

        private NetworkType networkType;

        private Dictionary<uint, Session> sessions = new Dictionary<uint, Session>();


        public NetworkService(NetworkType network)
        {
            this.networkType = network;

            Init();
        }

        public NetworkService(NetworkType network, string ip, int port)
        {
            this.networkType = network;

            Init(ip, port);
        }

        public Session GetSession(uint id)
        {
            if (sessions.TryGetValue(id, out Session session))
            {
                return session;
            }

            return null;
        }

        public Session CreateSession(string ip, int port)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            return CreateSession(endPoint);
        }

        public Session CreateSession(IPEndPoint endPoint)
        {
            BaseProtocol protocol = service.CreateProtocol(endPoint);
            protocol.Start();

            Session session = new Session(this, protocol);

            sessions[session.Id] = session;
            
            return session;
        }

        #region temp

        public void Broadcase(byte[] message)
        {
            sessions.Values.ToList().ForEach(x => { x.Transfer(message); });
        }


        public void Transfer(uint udpId, byte[] message)
        {
            if (sessions.TryGetValue(udpId, out Session session))
            {
                session.Transfer(message);
            }
        }

        #endregion

        public void Remove(uint id)
        {
            if (sessions.TryGetValue(id, out Session session))
            {
                session.Dispose();
                sessions.Remove(id);
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
            }
        }

        private void Init(string ip, int port)
        {
            IPEndPoint endPoint;
            switch (this.networkType)
            {
                case NetworkType.Tcp:
                    endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                    service = new TcpService(endPoint, OnAccept);
                    break;
                case NetworkType.Udp:
                    service = new UdpService(port, OnAccept);
                    break;
            }
        }

        private void OnAccept(BaseProtocol baseProtocol)
        {
            Session session = new Session(this, baseProtocol);
            baseProtocol.Start();

            sessions[session.Id] = session;
        }

        public void Dispose()
        {
        }
    }
}
