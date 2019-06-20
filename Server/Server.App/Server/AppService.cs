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

        public override void Init(AppOption option)
        {
            //框架的各种初始化工作
            base.Init(option);

            this.InitAppDiffence();
            this.InitDone();

            ConsoleReader.Instance.Start(DoCmd);
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

        public override void InitAppDiffence()
        {
            base.InitAppDiffence();

            switch (this.AppType)
            {
                case AppType.Gate:
                    this.AppInitGate();
                    break;
                case AppType.Manager:
                        this.AppInitManager();
                    break;
                case AppType.Map:
                        this.AppInitMap();
                    break;
                case AppType.Social:
                        this.AppInitSocial();
                    break;
                case AppType.AllServer:
                        this.AppInitAll();
                    break;
            }
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
                case "Test":
                    Benchmark.Test();
                    break;
                default:
                    Logger.Info($"system call -> {message}");
                    break;
            }
        }
    }
}
