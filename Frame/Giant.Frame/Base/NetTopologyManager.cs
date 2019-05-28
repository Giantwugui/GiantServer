using Giant.Data;
using Giant.Net;
using Giant.Share;
using System.Collections.Generic;

namespace Giant.Frame
{
    public class NetTopologyManager
    {
        //各app内部连接
        private ListMap<AppType, Session> appSessions = new ListMap<AppType, Session>();

        public BaseService Service { get; private set; }

        public AppType AppType { get { return Service.AppType; } }

        public List<Session> GateSessions { get { return appSessions[AppType.Gate]; } }
        public List<Session> ManagerSessions { get { return appSessions[AppType.Manager]; } }
        public List<Session> ZoneSessions { get { return appSessions[AppType.Zone]; } }
        public List<Session> SocialSessions { get { return appSessions[AppType.Social]; } }


        public NetTopologyManager(BaseService service)
        {
            this.Service = service;
        }

        public void Init()
        {
            Session session;
            var netPology = NetTopologyConfig.GetTopology(this.AppType);
            foreach (var kv in netPology)
            {
                session = this.Service.InnerNetworkService.GetSession(kv.InnerAddress);

                AddSession(kv.AppyType, session);
            }
        }

        public void AddSession(AppType appType, Session session)
        {
            appSessions.Add(appType, session);
        }
    }
}
