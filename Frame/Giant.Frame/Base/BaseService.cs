using Giant.Data;
using Giant.DB;
using Giant.Log;
using Giant.Net;
using Giant.Redis;
using Giant.Share;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Giant.Frame
{
    public class BaseService
    {
        public NetworkService NetworkService { get; protected set; }

        public int MainId { get; private set; }

        public AppyType AppType { get; set; }



        public virtual void Init()
        {
            SetConsoleCtrlHandler(cancelHandler, true);

            // 异步方法全部会回掉到主线程
            SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);

            //框架的各种初始化工作
            DataManager.Instance.LoadData();
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
            DBConfig.Init();
            ServerConfig.Init();
        }

        private void InitNetwork()
        {
            //网络服务
            this.NetworkService = new NetworkService(NetworkType.Tcp, "127.0.0.1:9091");
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
