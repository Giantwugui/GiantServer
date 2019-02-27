using StackExchange.Redis;
using System;

namespace Giant.Redis
{
    public class RedisManager
    {

        private RedisManager()
        {
            Configuration = new ConfigurationOptions();
            Configuration.EndPoints.Add("127.0.0.1:6379");

            //集群连接方式
            Configuration.EndPoints.Add("127.0.0.1:6380");

            Configuration.AbortOnConnectFail = false;
            Configuration.AllowAdmin = true;
            //Configuration.Password = "";

            ConnectRedisService();
        }

        private bool ConnectRedisService()
        {
            try
            {
                connection = ConnectionMultiplexer.Connect(Configuration);

                //注册如下事件
                connection.ConnectionFailed += MuxerConnectionFailed;
                connection.ConnectionRestored += MuxerConnectionRestored;
                connection.ErrorMessage += MuxerErrorMessage;
                connection.ConfigurationChanged += MuxerConfigurationChanged;
                connection.HashSlotMoved += MuxerHashSlotMoved;
                connection.InternalError += MuxerInternalError;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return connected;
        }

        #region 事件

        /// <summary>
        /// 配置更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConfigurationChanged(object sender, EndPointEventArgs e)
        {
            //log.InfoAsync($"Configuration changed: {e.EndPoint}");
        }

        /// <summary>
        /// 发生错误时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerErrorMessage(object sender, RedisErrorEventArgs e)
        {
            //log.InfoAsync($"ErrorMessage: {e.Message}");
        }

        /// <summary>
        /// 重新建立连接之前的错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            //log.InfoAsync($"ConnectionRestored: {e.EndPoint}");
        }

        /// <summary>
        /// 连接失败 ， 如果重新连接成功你将不会收到这个通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            //log.InfoAsync($"重新连接：Endpoint failed: {e.EndPoint},  {e.FailureType} , {(e.Exception == null ? "" : e.Exception.Message)}");
        }

        /// <summary>
        /// 更改集群
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerHashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            //log.InfoAsync($"HashSlotMoved:NewEndPoint{e.NewEndPoint}, OldEndPoint{e.OldEndPoint}");
        }

        /// <summary>
        /// redis类库错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerInternalError(object sender, InternalErrorEventArgs e)
        {
            //log.InfoAsync($"InternalError:Message{ e.Exception.Message}");
        }

        #endregion 事件


        public static RedisManager Instance { get; } = new RedisManager();

        /// <summary>
        /// 当前连接的Redis中的DataBase索引，默认0-16，可以在service.conf配置，最高64
        /// </summary>
        private int dataBaseIndex = 0;
        public int DataBaseIndex { get { return dataBaseIndex; } }

        /// <summary>
        /// 当前连接的Redis中连接字符串，格式为：127.0.0.1:6379,allowadmin=true,passowrd=pwd
        /// </summary>
        public ConfigurationOptions Configuration { get; set; }

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        private ConnectionMultiplexer connection;
        public ConnectionMultiplexer Connection
        {
            get
            {
                if (connection == null)
                {
                    throw new Exception("必须先调用 RedisManager.SetLinkParam 配置Redis连接信息!");
                }
                return connection;
            }
        }

        /// <summary>
        /// 连接状态
        /// </summary>
        private bool connected = false;
        public bool Connected { get { return connected; } set { connected = value; } }
    }
}
