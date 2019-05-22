using StackExchange.Redis;
using System;
using Giant.Log;

namespace Giant.Redis
{
    public class RedisService
    {
        public bool Connected { get; set; }

        /// <summary>
        /// 当前连接的Redis中的DataBase索引，默认0-16，可以在service.conf配置，最高64
        /// </summary>
        public int DataBaseIndex { get; private set; }

        public ConfigurationOptions Configuration { get; private set; }

        /// <summary>
        /// 当前连接的Redis中连接字符串，格式为：127.0.0.1:6379,allowadmin=true,passowrd=pwd
        /// </summary>
        public ConnectionMultiplexer Connection { get; private set; }

        public static RedisService Instance { get; } = new RedisService();


        private RedisService()
        {
            Configuration = new ConfigurationOptions();
            Configuration.EndPoints.Add("127.0.0.1:6379");

            //集群连接方式
            Configuration.EndPoints.Add("127.0.0.1:6380");

            Configuration.AbortOnConnectFail = false;
            Configuration.AllowAdmin = true;
            //Configuration.Password = "";

            Connect();
        }

        private bool Connect()
        {
            try
            {
                Connection = ConnectionMultiplexer.Connect(Configuration);

                //注册如下事件
                Connection.ConnectionFailed += ConnectionFailed;
                Connection.ConnectionRestored += ConnectionRestored;
                Connection.ConfigurationChanged += ConfigurationChanged;
                Connection.HashSlotMoved += HashSlotMoved;
                Connection.InternalError += InternalError;
                Connection.ErrorMessage += ErrorMessage;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return this.Connected;
        }

        #region 事件

        /// <summary>
        /// 配置更改时
        /// </summary>
        private static void ConfigurationChanged(object sender, EndPointEventArgs e)
        {
            Logger.Warn($"Configuration changed: {e.EndPoint}");
        }

        /// <summary>
        /// 重新建立连接之前的错误
        /// </summary>
        private static void ConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            Logger.Warn($"ConnectionRestored: {e.EndPoint}");
        }

        /// <summary>
        /// 连接失败 ， 如果重新连接成功你将不会收到这个通知
        /// </summary>
        private static void ConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            Logger.Warn($"重新连接：Endpoint failed: {e.EndPoint},  {e.FailureType} , {(e.Exception == null ? "" : e.Exception.Message)}");
        }

        /// <summary>
        /// 更改集群
        /// </summary>
        private static void HashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            Logger.Warn($"HashSlotMoved:NewEndPoint{e.NewEndPoint}, OldEndPoint{e.OldEndPoint}");
        }

        /// <summary>
        /// redis类库错误
        /// </summary>
        private static void InternalError(object sender, InternalErrorEventArgs e)
        {
            Logger.Error($"InternalError:Message{ e.Exception.Message}");
        }

        /// <summary>
        /// 发生错误时
        /// </summary>
        private static void ErrorMessage(object sender, RedisErrorEventArgs e)
        {
            Logger.Error($"ErrorMessage: {e.Message}");
        }

        #endregion 事件
    }
}
