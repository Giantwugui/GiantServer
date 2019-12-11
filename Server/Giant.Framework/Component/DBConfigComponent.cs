using Giant.Core;

namespace Giant.Framework
{
    public class DBConfigComponent : InitSystem, ILoad
    {
        public DBConfig DBConfig { get; private set; }
        public RedisConfig RedisConfig { get; private set; }

        public override void Init()
        {
            Data data = DataComponent.Instance.GetData("DBConfig", 1);

            DBConfig = new DBConfig
            {
                DBHost = data.GetString("DBHost"),
                DBName = data.GetString("DBName"),
                Account = data.GetString("Account"),
                Pwd = data.GetString("Pwd"),
                TaskCount = data.GetInt("TaskCount"),
            };

            RedisConfig = new RedisConfig
            {
                Host = data.GetString("RedisHost"),
                Pwd = data.GetString("RedisPwd"),
                DBIndex = data.GetInt("RedisIndex")
            };
        }

        public void Load()
        {
            Init();
        }
    }
}
