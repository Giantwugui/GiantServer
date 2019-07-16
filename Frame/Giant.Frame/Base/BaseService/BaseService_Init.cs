using Giant.Data;
using Giant.DB;
using Giant.Log;
using Giant.Net;
using Giant.Redis;
using Giant.Share;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace Giant.Frame
{
    public partial class BaseService
    {
        public virtual void Init(AppOption option)
        {
            this.AppOption = option;
            this.AppState = AppState.Starting;

            //框架的各种初始化工作
            this.InitBase();
            this.InitLogConfig();

            this.InitData();
            this.InitNetwork();
            this.InitProtocol();
            this.InitDBService();
            this.InitRedisService();
            this.InitNetworkTopology();
        }

        public virtual void InitData()
        {
            DataManager.Instance.Init();

            DBConfig.Init();
            AppConfigLibrary.Init();
            NetTopologyConfig.Init();
        }

        public virtual void InitAppDiffence()
        {
        }

        private void InitBase()
        {
            //窗口关闭事件
            SetConsoleCtrlHandler(cancelHandler, true);

            // 异步方法全部会回掉到主线程
            SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);

            IdGenerator.AppId = this.AppId;
        }

        //日志配置
        private void InitLogConfig()
        {
            Logger.Init(false, this.AppType.ToString(), this.AppId);
        }

        //网络服务
        private void InitNetwork()
        {
            AppConfig config = AppConfigLibrary.GetNetConfig(this.AppId);
            this.InnerNetworkService = new InnerNetworkService(NetworkType.Tcp, config.InnerAddress);

            //部分App只有内部服务，Zone
            if (!string.IsNullOrEmpty(config.OutterAddress))
            {
                this.OutterNetworkService = new OutterNetworkService(NetworkType.Tcp, config.InnerAddress);
            }
        }

        //注册消息响应
        private void InitProtocol()
        {
            Assembly properMsgAssembly = Assembly.GetEntryAssembly();
            this.InnerNetworkService.MessageDispatcher.RegisterHandler(this.AppType, properMsgAssembly);

            if (this.OutterNetworkService != null)
            {
                this.OutterNetworkService.MessageDispatcher.RegisterHandler(this.AppType, properMsgAssembly);
            }
        }

        //数据库服务
        private void InitDBService()
        {
            if (this.AppType.NeedDBService())
            {
                DataBaseService.Instance.Init(DataBaseType.MongoDB, DBConfig.DBHost, DBConfig.DBName,
                    DBConfig.DBAccount, DBConfig.DBPwd, DBConfig.DBTaskCount);
            }
        }

        //Redis服务
        private void InitRedisService()
        {
            if (this.AppType.NeedRedisServer())
            {
                RedisService.Instance.Init(DBConfig.RedisHost, DBConfig.RedisPwd, DBConfig.RedisTaskCount, 0);
            }
        }

        //网络拓扑
        private void InitNetworkTopology()
        {
            this.NetProxyManager.Init(this);
        }


        #region 窗口关闭事件

        delegate bool ControlCtrlHandle(int ctrlType);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ControlCtrlHandle HandlerRoutine, bool Add);
        private static readonly ControlCtrlHandle cancelHandler = new ControlCtrlHandle(HandleMathord);

        private static bool HandleMathord(int ctrlType)
        {
            switch (ctrlType)
            {
                case 0:
                    Logger.Warn("无法使用 Ctrl+C 强制关闭窗口"); //Ctrl+C关闭
                    return true;
                case 2:
                    Logger.Warn("工具被强制关闭");//按控制台关闭按钮关闭
                    return true;
            }

            return false;
        }

        #endregion
    }
}
