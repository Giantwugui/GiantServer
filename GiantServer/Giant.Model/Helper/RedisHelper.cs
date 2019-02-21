using StackExchange.Redis;
using System;

namespace Giant.Model.Helper
{
    public class RedisHelper
    {
        public bool SetLinkParam(string host, int port)
        {
            Configuration = $"{host}:{port}";

            return ConnectRedisService();
        }

        public bool SetLinkParam(string configuration)
        {
            Configuration = configuration;

            return ConnectRedisService();
        }

        public async void AddString(string key, string value)
        {
            await database.StringSetAsync(key, value);
        }

        public string Get(string key)
        {
            return database.StringGet(key);
        }

        public string HashGet(string redisKey, string hashKey)
        {
            return database.HashGet(redisKey, hashKey);
        }

        public void HashSet(string redisKey, string hashKey, string hashValue)
        {
           database.HashSet(redisKey, hashKey, hashValue);
        }


        private RedisHelper() { }

        private bool ConnectRedisService()
        {
            try
            {
                ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(Configuration);

                database = connection.GetDatabase();

                connected = database != null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return connected;
        }


        

        public static RedisHelper Instance { get; } = new RedisHelper();

        /// <summary>
        /// 连接配置
        /// </summary>
        public string Configuration { get; set; }

        /// <summary>
        /// 连接状态
        /// </summary>
        private bool connected = false;
        public bool Connected { get { return connected; } set { connected = value; } }

        private IDatabase database;
    }
}
