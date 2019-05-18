using Giant.Share;

namespace Giant.Frame
{
    public class UidGenerator
    {
        private static ushort value;
        private static long instanceId;
        private static long nowSecond = TimeHelper.NowSeconds;

        public static long MainId
        {
            set
            {
                instanceId = value << 48;
            }
        }

        public static long NewUid
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
    }
}
