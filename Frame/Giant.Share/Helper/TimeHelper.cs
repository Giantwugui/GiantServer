using System;

namespace Giant.Share
{
    public class TimeHelper
    {
        public const string TIMESTR = "yyyyMMddHHmmss";
        public const string TIMESTR1 = "yyyy-MM-dd HH:mm:ss";

        private static readonly DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly long startTimeTicks = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

        public static long NowMilliSeconds => (Now.Ticks - startTimeTicks) / 10000;

        public static long NowSeconds => (Now.Ticks - startTimeTicks) / 10000000;

        public static DateTime Now => DateTime.Now;

        public static string NowString { get { return Now.ToString(TIMESTR); } }

        public static string NowString1 { get { return Now.ToString(TIMESTR1); } }


    }
}
