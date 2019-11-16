using Giant.Log;
using Giant.Share;
using Server.Frame;
using System;
using System.Threading;

namespace Server.Manager
{
    public class AppService : BaseAppService
    {
        public static AppService Instacne { get; } = new AppService();

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
