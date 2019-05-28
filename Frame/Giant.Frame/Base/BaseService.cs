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
        public InnerNetworkService InnerNetworkService { get; private set; }
        public OutterNetworkService OutterNetworkService { get; private set; }

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
            this.InitProtocol();
            this.InitDBService();
            this.InitRedisService();
        }

        public virtual void Update()
        {
            try
            {
                OneThreadSynchronizationContext.Instance.Update();

                this.InnerNetworkService.Update();
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

        public virtual void InitDone()
        {
        }

        //日志配置
        private void InitLogConfig()
        {
            Logger.Init(false, this.AppType.ToString(), this.AppId);
        }

        //网络服务
        private void InitNetwork()
        {
            NetConfig config = ServerConfig.GetNetConfig(this.AppType, this.SubId);
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
            Assembly properMsgAssembly = Assembly.GetEntryAssembly();//特有消息处理程序及(Giant.App)
            Assembly golobalAssembly = Assembly.GetExecutingAssembly();//全局消息处理程序集(Giant.Frame)

            this.InnerNetworkService.MessageDispatcher.RegisterHandler(this.AppType, golobalAssembly);
            this.InnerNetworkService.MessageDispatcher.RegisterHandler(this.AppType, properMsgAssembly);

            if (this.OutterNetworkService != null)
            {
                this.OutterNetworkService.MessageDispatcher.RegisterHandler(this.AppType, golobalAssembly);
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
