using Giant.Util;

namespace Giant.Core
{
    public class IdGenerator
    {
        private static ushort value;
        private static long instanceId;
        private static long nowSecond = TimeHelper.NowSeconds;

        public static long AppId
        {
            set { instanceId = value << 48; }
        }

        public static long NewId
        {
            get
            {
                long now = TimeHelper.NowSeconds;
                if (now != nowSecond)
                {
                    nowSecond = now;
                    value = 0;
                }

                return instanceId + (nowSecond << 16) + ++value;
            }
        }

        public static int GetAppId(long uid)
        {
            return (int)(uid >> 48);
        }

    }
}
