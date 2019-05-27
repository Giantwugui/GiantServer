using Giant.Frame;
using Giant.Log;
using Giant.Net;
using Giant.Share;

namespace Server.App
{
    public partial class Service : BaseService
    {
        public static Service Instacne { get; } = new Service();

        private Service()
        {
            this.ServerType = ServerType.Gate;
        }

        public override void Init()
        {
            //框架的各种初始化工作
            base.Init();

            ConsoleReader.Instance.Start(DoCmd);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void InitData()
        {
            base.InitData();
        }

        private void DoCmd(string message)
        {
            switch (message)
            {
                default:
                    Logger.Info($"read message -> {message}");
                    break;
            }
        }
    }
}
