using Giant.Data;
using Giant.DB;
using Giant.Log;
using Giant.Net;
using Giant.Redis;
using Giant.Share;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Reflection;

namespace Giant.Frame
{
    public abstract class BaseService
    {
        public NetworkService NetworkService { get; protected set; }

        public int AppId { get; private set; }
        public int SubId { get; private set; }
        public AppyType AppType { get; private set; }

        public virtual void Init(AppyType appyType, int appId, int subId)
        {
            this.AppType = appyType;
            this.AppId = appId;
            this.SubId = subId;

            SetConsoleCtrlHandler(cancelHandler, true);

            // 异步方法全部会回掉到主线程
            SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);

            //框架的各种初始化工作
            this.InitLogConfig();

            this.InitData();
            this.InitNetwork();
            this.InitDBService();
            this.InitRedisService();
        }

        public virtual void Update()
        {
            try
            {
                OneThreadSynchronizationContext.Instance.Update();

                this.NetworkService.Update();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public virtual void InitData()
        {
            DataManager.Instance.LoadData();

            DBConfig.Init();
            ServerConfig.Init();
        }

        private void InitLogConfig()
        {
            Logger.Init(false, this.AppType.ToString(), this.AppId);
        }

        private void InitNetwork()
        {
            //网络服务
            NetConfig config = ServerConfig.GetNetConfig(this.AppType, this.SubId);
            this.NetworkService = new NetworkService(NetworkType.Tcp, config.Address);

            //注册消息响应
            this.NetworkService.MessageDispatcher.RegisterHandler(this.AppType, Assembly.GetEntryAssembly());
            this.NetworkService.MessageDispatcher.RegisterHandler(this.AppType, Assembly.GetExecutingAssembly());//通用的消息处理可以放在 "Giant.Frame" 程序集
        }

        private void InitDBService()
        {
            //数据库服务
            if (this.AppType.NeedDBService())
            {
                DataBaseService.Instance.Init(DataBaseType.MongoDB, DBConfig.DBHost, DBConfig.DBName,
                    DBConfig.DBAccount, DBConfig.DBPwd, DBConfig.DBTaskCount);
            }
        }

        private void InitRedisService()
        {
            //Redis服务
            if (this.AppType.NeedRedisServer())
            {
                RedisService.Instance.Init(DBConfig.RedisHost, DBConfig.RedisPwd, DBConfig.RedisTaskCount, 0);
            }
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
