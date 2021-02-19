namespace Giant.Core
{
    public static class AppHelper
    {
        public static bool IsSame(this AppType appyType, AppType otherApp)
        {
            return (appyType & otherApp) > 0;
        }

        public static bool NeedDBService(this AppType appyType)
        {
            switch (appyType)
            {
                case AppType.Zone:
                case AppType.Gate:
                case AppType.AllServer:
                case AppType.Manager:
                case AppType.Relation:
                case AppType.Account:
                    return true;
            }
            return false;
        }

        public static bool NeedRedisServer(this AppType appyType)
        {
            switch (appyType)
            {
                case AppType.Gate:
                    return false;
                case AppType.Zone:
                case AppType.AllServer:
                case AppType.Manager:
                case AppType.Relation:
                    return true;
            }
            return false;
        }
    }
}
