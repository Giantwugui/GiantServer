using Giant.Data;
using Giant.Log;
using Giant.Net;
using Giant.Share;
using System;
using System.Runtime.InteropServices;

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
                OneThreadSynchronizationContext.Instance.Update();

                Timer.Instance.Update();//定时器

                this.InnerNetworkService.Update();
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
       
    }
}
