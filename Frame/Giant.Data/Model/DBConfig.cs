using Giant.Log;

namespace Giant.Data
{
    public class DBConfig
    {
        public static string DBHost { get; private set; }
        public static string DBName { get; private set; }
        public static string DBAccount { get; private set; }
        public static string DBPwd { get; private set; }
        public static int DBTaskCount { get; private set; }

        public static string RedisHost { get; private set; }
        public static string RedisPwd { get; private set; }
        public static int RedisTaskCount { get; private set; }

        public static void Init()
        {
            Data data = DataManager.Instance.GetData("DBConfig", 1);
            if (data == null)
            {
                Logger.Error("Can not find XML DBConfig");
                return;
            }

            DBHost = data.GetString("DBHost");
            DBName = data.GetString("DBName");
            DBAccount = data.GetString("DBAccount");
            DBPwd = data.GetString("DBPwd");
            DBTaskCount = data.GetInt("DBTaskCount");

            RedisHost = data.GetString("RedisHost");
            RedisPwd = data.GetString("RedisPwd");
            RedisTaskCount = data.GetInt("RedisTaskCount");
        }
    }
}
