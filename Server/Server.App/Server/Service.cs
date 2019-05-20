using Giant.DB;
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

            //数据库服务
            DBService.Instance.Init(DataBaseType.MongoDB, "127.0.0.1:27017", "Giant", "dbOwner", "dbOwner");

            //网络服务
            this.NetworkService = new NetworkService(NetworkType.Tcp, "127.0.0.1:9091");

            ConsoleReader.Instance.Start(DoCmd);
        }

        public override void Update()
        {
            base.Update();
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
