using Giant.Frame;
using Giant.Log;
using Giant.Share;
using System;

namespace Server.App
{
    public partial class AppService : BaseService
    {
        public static AppService Instacne { get; } = new AppService();

        private AppService() { }

        public override void Init(AppType appyType, int appId)
        {
            //框架的各种初始化工作
            base.Init(appyType, appId);

            this.InitDone();
            ConsoleReader.Instance.Start(DoCmd);
        }

        public override void Update()
        {
            try
            {
                base.Update();
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
            this.AppState = AppState.Started;
        }

        private void DoCmd(string message)
        {
            switch (message)
            {
                default:
                    Logger.Info($"system call -> {message}");
                    break;
            }
        }
    }
}
