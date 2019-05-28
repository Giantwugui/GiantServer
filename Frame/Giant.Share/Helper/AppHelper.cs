namespace Giant.Share
{
    public static class AppHelper
    {
        public static bool IsSame(this AppyType appyType, AppyType otherApp)
        {
            return (appyType & otherApp) > 0;
        }


        public static bool NeedDBService(this AppyType appyType)
        {
            switch (appyType)
            {
                case AppyType.Gate:
                case AppyType.Manager:
                case AppyType.Zone:
                    return true;
            }
            return false;
        }

        public static bool NeedRedisServer(this AppyType appyType)
        {
            switch (appyType)
            {
                case AppyType.Gate:
                    return false;
                case AppyType.Manager:
                case AppyType.Zone:
                    return true;
            }
            return false;
        }
    }
}
