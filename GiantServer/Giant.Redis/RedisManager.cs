using StackExchange.Redis;
using System;

namespace Giant.Redis
{

    public class RedisManager
    {

        private RedisManager()
        {
            Configuration = $"127.0.0.1:6379";

            ConnectRedisService();
        }

        private bool ConnectRedisService()
        {
            try
            {
                connection = ConnectionMultiplexer.Connect(Configuration);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return connected;
        }
        

        public static RedisManager Instance { get; } = new RedisManager();

        /// <summary>
        /// 当前连接的Redis中的DataBase索引，默认0-16，可以在service.conf配置，最高64
        /// </summary>
        private int dataBaseIndex = 1;
        public int DataBaseIndex { get { return dataBaseIndex; } }

        /// <summary>
        /// 当前连接的Redis中连接字符串，格式为：127.0.0.1:6379,allowadmin=true,passowrd=pwd
        /// </summary>
        public string Configuration { get; set; }

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
