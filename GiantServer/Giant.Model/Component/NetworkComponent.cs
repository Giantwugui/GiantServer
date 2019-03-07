using System;
using System.Collections.Generic;
using System.Net;

namespace Giant.Model
{
    public enum NetServiceType
    {
        TCP,
        UDP,
        HTTP
    }

    class NetworkComponent : Component
    {
        private BaseService mService;

        private Dictionary<long, Session> sessionDict = new Dictionary<long, Session>();


        private void OnAccept(BChannel channel)
        {
            Session session = ComponentFactory.CreateWithParent<Session, BChannel>(this, channel, false);

            sessionDict[session.Id] = session;

            session.Start();
        }

        public void Start(NetServiceType netServiceType, string address, int port)
        {
            try
            {
                IPEndPoint ipEndPoint = null;
                switch (netServiceType)
                {
                    case NetServiceType.TCP:
                        ipEndPoint = NetHelper.ToIPEndPoint(address, port);
                        mService = new TService(ipEndPoint, OnAccept);
                        break;
                    case NetServiceType.UDP:
                        ipEndPoint = NetHelper.ToIPEndPoint(address, port);
                        break;
                    case NetServiceType.HTTP:
                        break;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("NetService StartError", ex);
            }
        }


        public void Remove(long id)
        {
            if (sessionDict.TryGetValue(id, out Session session))
            {
                //掉线相关处理
                sessionDict.Remove(session.Id);
            }
        }



    }
}
