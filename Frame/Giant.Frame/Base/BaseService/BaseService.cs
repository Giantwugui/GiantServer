using Giant.Log;
using Giant.Msg;
using Giant.Net;
using Giant.Share;
using System;

namespace Giant.Frame
{
    public abstract partial class BaseService
    {
        public NetProxyManager NetProxyManager { get; private set; }
        public InnerNetworkService InnerNetworkService { get; private set; }
        public OutterNetworkService OutterNetworkService { get; private set; }

        public AppState AppState { get; protected set; }
        public AppOption AppOption { get; private set; }

        public AppType AppType => AppOption.AppType;
        public int AppId => AppOption.AppId;

        public virtual void Update(float dt)
        {
            try
            {
                OneThreadSynchronizationContext.Instance.Update();//异步回调处理

                Timer.Instance.Update();//定时器

                this.InnerNetworkService.Update();
                this.NetProxyManager.Update(dt);
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
