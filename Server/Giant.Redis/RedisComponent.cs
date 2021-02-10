using Giant.Core;
using Giant.Logger;
using Giant.Model;
using StackExchange.Redis;
using System;

namespace Giant.Redis
{
    public class RedisComponent : InitSystem<RedisConfig>, IRedisService
    {
        public TaskPool TaskPool { get; private set; }

        public bool Connected { get; set; }

        /// <summary>
        /// 当前连接的Redis中的DataBase索引，默认0-16，可以在service.conf配置，最高64
        /// </summary>
        public int DBIndex { get; private set; }

        public ConfigurationOptions Configuration { get; private set; }

        /// <summary>
        /// 当前连接的Redis中连接字符串，格式为：127.0.0.1:6379,allowadmin=true,passowrd=pwd
        /// </summary>
        public ConnectionMultiplexer Connection { get; private set; }

        public IDatabase Database { get; private set; }

        public static RedisComponent Instance { get; } = new RedisComponent();

        public override void Init(RedisConfig config)
        {
            string[] hosts = config.Host.Split(',', StringSplitOptions.RemoveEmptyEntries);
            Configuration = new ConfigurationOptions()
            {
                AbortOnConnectFail = false,
                AllowAdmin = true,
                Password = config.Pwd
            };

            foreach (var curr in hosts)
            {
                Configuration.EndPoints.Add(curr);
            }

            DBIndex = config.DBIndex;

            TaskPool = new TaskPool(1);
            TaskPool.Start();

            Connect();
        }

        private bool Connect()
        {
            try
            {
                Connection = ConnectionMultiplexer.Connect(Configuration);
                Database = Connection.GetDatabase(DBIndex);
                Connected = true;

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
                Log.Error(ex);
            }

            return Connected;
        }

        #region 事件

        /// <summary>
        /// 配置更改时
        /// </summary>
        private static void ConfigurationChanged(object sender, EndPointEventArgs e)
        {
            Log.Warn($"Configuration changed: {e.EndPoint}");
        }

        /// <summary>
        /// 重新建立连接之前的错误
        /// </summary>
        private static void ConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            Log.Warn($"ConnectionRestored: {e.EndPoint}");
        }

        /// <summary>
        /// 连接失败 ， 如果重新连接成功你将不会收到这个通知
        /// </summary>
        private static void ConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            Log.Warn($"重新连接：Endpoint failed: {e.EndPoint},  {e.FailureType} , {(e.Exception == null ? "" : e.Exception.Message)}");
        }

        /// <summary>
        /// 更改集群
        /// </summary>
        private static void HashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            Log.Warn($"HashSlotMoved:NewEndPoint{e.NewEndPoint}, OldEndPoint{e.OldEndPoint}");
        }

        /// <summary>
        /// redis类库错误
        /// </summary>
        private static void InternalError(object sender, InternalErrorEventArgs e)
        {
            Log.Error($"InternalError:Message{ e.Exception.Message}");
        }

        /// <summary>
        /// 发生错误时
        /// </summary>
        private static void ErrorMessage(object sender, RedisErrorEventArgs e)
        {
            Log.Error($"ErrorMessage: {e.Message}");
        }

        #endregion 事件
    }
}