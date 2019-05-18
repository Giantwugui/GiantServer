using Giant.Frame;
using Giant.Log;
using Giant.Share;

namespace Server.App
{
    public class Service : BaseService
    {
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

        public void DoCmd(string message)
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
