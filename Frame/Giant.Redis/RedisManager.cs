using StackExchange.Redis;

namespace Giant.Redis
{
    public class RedisManager
    {
        /// <summary>
        /// 当前连接的Redis中的DataBase索引，默认0-16，可以在service.conf配置，最高64
        /// </summary>
        public int DataBaseIndex { get; private set; }

        /// <summary>
        /// 当前连接的Redis中连接字符串，格式为：127.0.0.1:6379,allowadmin=true,passowrd=pwd
        /// </summary>
        public ConnectionMultiplexer Connection => this.RedisService.Connection;

        public static RedisManager Instance { get; } = new RedisManager();

        private readonly RedisService RedisService;

        private RedisManager()
        {
            this.DataBaseIndex = 0;
            RedisService = RedisService.Instance;
            RedisService.Instance.Init("127.0.0.1:6379,127.0.0.1:6380", "", this.DataBaseIndex);
        }
    }
}
