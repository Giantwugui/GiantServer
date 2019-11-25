using Giant.Data;
using Giant.DB;
using Giant.Log;
using Giant.Net;
using Giant.Redis;
using Giant.Share;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace Server.Frame
{
    public partial class BaseAppService
    {
        public abstract void Start(string[] args);

        public virtual void Init(string[] args)
        {
            //框架的各种初始化
            InitBase(args);
            InitLogConfig();

            InitData();
            InitNetwork();
            InitProtocol();
            InitDBService();
            InitRedisService();
            InitServerFactory();
            InitNetworkTopology();
        }

        public virtual void InitData()
        {
            DataManager.Instance.Init();

            DBConfig.Init();
            AppConfigLibrary.Init();
            NetTopologyLibrary.Init();
        }

        protected virtual void OnAccept(Session session, bool isConnect) { }

        protected virtual void InitServerFactory()
        {
            ServerFactory = new BaseServerFactory(this);
        }


        private void InitBase(string[] args)
        {
            Framework.Init(this, args);
            Framework.AppState = AppState.Starting;

            //窗口关闭事件
            SetConsoleCtrlHandler(cancelHandler, true);

            // 异步方法全部会回掉到主线程
            SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);

            IdGenerator.AppId = AppId;
        }

        //日志配置
        private void InitLogConfig()
        {
            Logger.Init(false, AppType.ToString(), AppId, SubId);
        }

        //网络服务
        private void InitNetwork()
        {
            AppConfig = AppConfigLibrary.GetNetConfig(AppType, AppId, SubId);

            if(!string.IsNullOrEmpty(AppConfig.InnerAddress))
            {
                InnerNetworkService = new InnerNetworkService(NetworkType.Tcp, AppConfig.InnerAddress);
            }

            if (!string.IsNullOrEmpty(AppConfig.OutterAddress))
            {
                OutterNetworkService = new OutterNetworkService(NetworkType.Tcp, AppConfig.OutterAddress, OnAccept);
            }
        }

        //注册消息响应
        private void InitProtocol()
        {
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            Assembly currendAssembly = Assembly.GetExecutingAssembly();

            if (InnerNetworkService != null)
            {
                InnerNetworkService.MessageDispatcher.RegisterHandler(entryAssembly);
                InnerNetworkService.MessageDispatcher.RegisterHandler(currendAssembly);
            }

            if (OutterNetworkService != null)
            {
                OutterNetworkService.MessageDispatcher.RegisterHandler(entryAssembly);
                OutterNetworkService.MessageDispatcher.RegisterHandler(currendAssembly);
            }
        }

        //数据库服务
        private void InitDBService()
        {
            if (AppType.NeedDBService())
            {
                DataBaseService.Instance.Init(DataBaseType.MongoDB, DBConfig.DBHost, DBConfig.DBName,
                    DBConfig.DBAccount, DBConfig.DBPwd, DBConfig.DBTaskCount);
            }
        }

        //Redis服务
        private void InitRedisService()
        {
            if (AppType.NeedRedisServer())
            {
                RedisService.Instance.Init(DBConfig.RedisHost, DBConfig.RedisPwd, DBConfig.RedisTaskCount, 0);
            }
        }

        //网络拓扑
        private void InitNetworkTopology()
        {
            NetProxyManager = new NetProxyManager(this);
            NetProxyManager.Init();
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
