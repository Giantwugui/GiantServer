using Giant.Data;
using Giant.Log;
using Giant.Msg;
using Giant.Net;
using Giant.Share;
using System;

namespace Giant.Frame
{
    public abstract partial class BaseService
    {
        public NetTopologyManager NetTopologyManager { get; private set; }
        public InnerNetworkService InnerNetworkService { get; private set; }
        public OutterNetworkService OutterNetworkService { get; private set; }

        public AppState AppState { get; set; }

        public AppType AppType { get; private set; }
        public int AppId { get; private set; }
        public int SubId { get; private set; }

        public virtual void InitData()
        {
            DataManager.Instance.LoadData();

            AppConfig.Init();
            DBConfig.Init();
            NetConfig.Init();
            NetTopologyConfig.Init();
        }

        public virtual void Update()
        {
            try
            {
                OneThreadSynchronizationContext.Instance.Update();//异步回调处理

                Timer.Instance.Update();//定时器

                this.InnerNetworkService.Update();
                this.HeartBeat();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public virtual void InitDone()
        {
        }

        public virtual void StopApp()
        {
            this.AppState = AppState.Stopping;
        }


        private long lastHeatBeatTime = TimeHelper.NowSeconds;
        private void HeartBeat()
        {
            if (TimeHelper.NowSeconds - lastHeatBeatTime > 10)
            {
                HeartBeat_Ping ping = new HeartBeat_Ping
                {
                    AppType = (int)this.AppType,
                    AppId = this.AppId,
                    SubId = this.SubId
                };

                this.InnerNetworkService.HeartBeat(ping);
                lastHeatBeatTime = TimeHelper.NowSeconds;
            }
        }
    }
}
