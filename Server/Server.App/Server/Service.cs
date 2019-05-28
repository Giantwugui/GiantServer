using Giant.Data;
using Giant.Frame;
using Giant.Log;
using Giant.Share;
using System;

namespace Server.App
{
    public partial class Service : BaseService
    {
        public static Service Instacne { get; } = new Service();

        private Service()
        {
        }

        public override void Init(AppyType appyType, int appId, int subId)
        {
            //框架的各种初始化工作
            base.Init(appyType, appId, subId);

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
