using System;

namespace Giant.Net
{
    public class TimeHelper
    {
        private static DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static long startTimeTicks = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

        public static long NowMilliSeconds => (Now.Ticks - startTimeTicks) / 10000;

        public static long NowSeconds => (Now.Ticks - startTimeTicks) / 10000000;

        public static DateTime Now => DateTime.Now;

        public static string NowString { get { return Now.ToString("yyyy-MM-dd hh:mm:ss"); } }
    }
}
