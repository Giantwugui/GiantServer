using Server.Frame;
using Giant.Log;
using Giant.Share;
using System;
using System.Threading;

namespace Server.Gate
{
    public class AppService : BaseAppService
    {
        public static AppService Instacne { get; } = new AppService();

        public BaseService GlobalServer => NetProxyManager.GetBackendSinglePoint(AppType.Global, this.AppId);
        public BaseService AccountServer => NetProxyManager.GetBackendSinglePoint(AppType.Account, this.AppId);
        public BaseService MapServer => NetProxyManager.GetBackendSinglePoint(AppType.Map, this.AppId);
        public BaseService ManagerServer => NetProxyManager.GetBackendSinglePoint(AppType.Map, this.AppId);


        public override void Start(string[] args)
        {
            this.Init(args);

            Logger.Warn($"server start complete------------- appType {Framework.AppType} appId {Framework.AppId}");

            this.DoUpdate();
        }

        public override void Init(string[] args)
        {
            //框架的各种初始化工作
            base.Init(args);
            this.ServerCreater = new BaseServerCreater(this.NetProxyManager);


            this.InitDone();

            ConsoleReader.Instance.Start(DoCmd);
        }

        private void DoUpdate()
        {
            while (true)
            {
                Thread.Sleep(1);

                this.Update(1 * 0.01f);
            }
        }

        public override void Update(float dt)
        {
            try
            {
                base.Update(dt);
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
