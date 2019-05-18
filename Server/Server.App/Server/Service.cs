using Giant.DB;
using Giant.Frame;
using Giant.Log;
using Giant.Net;
using Giant.Share;
using System.Reflection;

namespace Server.App
{
    public partial class Service : BaseService
    {
        private readonly DBService dbService = new DBService(DataBaseType.MongoDB);

        public int MainId { get; private set; }

        public override void Init()
        {
            //框架的各种初始化工作
            base.Init();

            //数据库服务
            dbService.Start("127.0.0.1:27017", "Giant", "dbOwner", "dbOwner");

            //网络服务
            this.NetworkService = new NetworkService(NetworkType.Tcp, "127.0.0.1:9091");

            //绑定消息响应
            BindResponser(Assembly.GetExecutingAssembly());

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
