using Giant.Log;
using Giant.Net;
using Giant.Share;
using Server.Frame;
using System;
using System.Threading;

namespace Server.Gate
{
    public partial class AppService : BaseAppService
    {
        public static AppService Instacne { get; } = new AppService();
        public GlobalServer GlobalServer => NetProxyManager.GetBackendSinglePoint(AppType.Global, AppId) as GlobalServer;
        public AccountServer AccountServer => NetProxyManager.GetBackendSinglePoint(AppType.Account, AppId) as AccountServer;
        public ZoneServer ZoneServer => NetProxyManager.GetBackendSinglePoint(AppType.Map, AppId) as ZoneServer;
        public ManagerServer ManagerServer => NetProxyManager.GetFrontendSinglePoint(AppType.Manager, AppId) as ManagerServer;

        public FrontendServerManager ZoneServerManager => NetProxyManager.GetFrontendServiceManager(AppType.Map);

        public override void Start(string[] args)
        {
            Init(args);

            Logger.Warn($"server start complete------------- appType {Framework.AppType} appId {Framework.AppId}");

            DoUpdate();
        }

        public override void Init(string[] args)
        {
            //框架的各种初始化工作
            base.Init(args);

            InitDone();

            ConsoleReader.Instance.Start(DoCmd);
        }

        protected override void InitServerFactory()
        {
            ServerFactory = new ServerFactory(this);
        }

        private void DoUpdate()
        {
            while (true)
            {
                Thread.Sleep(1);

                Update(1 * 0.01f);
            }
        }

        public override void Update(float dt)
        {
            try
            {
                base.Update(dt);
                ClientManager.Instance.Update();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public override void InitData()
        {
            base.InitData();
        }

        public override void InitDone()
        {
            base.InitDone();
        }

        public ZoneServer GetZoneServer(int appId, int subId)
        {
            return ZoneServerManager.GetService(appId, subId) as ZoneServer;
        }


        protected override void OnAccept(Session session, bool isConnect)
        {
            if (!isConnect)
            {
                ClientManager.Instance.ClientOffline(session);
            }
            else
            {
            }
        }

        private void DoCmd(string message)
        {
            string[] param = message.Split(' ');
            switch (param[0])
            {
                case "Test":
                    break;
                default:
                    Logger.Info($"system call -> {message}");
                    break;
            }
        }
    }
}
