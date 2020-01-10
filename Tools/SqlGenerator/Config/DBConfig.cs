using Giant.Core;

namespace SqlGenerator
{
    public class DBConfig
    {
        public static string DBHost { get; private set; }
        public static string DBName { get; private set; }
        public static string Account { get; private set; }
        public static string Pwd { get; private set; }

        public static void Init()
        {
            DataModel data = DataManager.Instance.GetData("DBConfig", 1);

            DBHost = data.GetString("DBHost");
            DBName = data.GetString("DBName");
            Account = data.GetString("Account");
            Pwd = data.GetString("Pwd");
        }
    }
}
