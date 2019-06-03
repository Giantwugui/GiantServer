using Giant.Log;
using Giant.Share;

namespace Giant.Data
{
    public class AppConfig
    {
        public static AppType AppType { get; private set; }
        public static int MainId { get; private set; }
        public static int SubId { get; private set; }

        public static void Init()
        {
            Data data = DataManager.Instance.GetData("AppConfig", 1);
            if (data == null)
            {
                Logger.Error("Can not find XML ServerConfig");
                return;
            }

            AppType = (AppType)data.GetInt("AppType");
            MainId = data.GetInt("MainId");
            SubId = data.GetInt("SubId");
        }


    }
}
