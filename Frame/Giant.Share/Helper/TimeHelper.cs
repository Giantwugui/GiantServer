using System;

namespace Giant.Share
{
    public class TimeHelper
    {
        public const string TimeStr = "yyMMddHHmmss";
        public const string TimeStr1 = "yyyy-MM-dd HH:mm:ss";
        public const string TimeStrWithSeconds = "yyyy-MM-dd HH:mm:ss.fff";

        private static readonly DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();
        private static readonly long startTimeTicks = startTime.Ticks;

        public static DateTime Now => DateTime.Now;
        public static long NowMilliSeconds => (Now.Ticks - startTimeTicks) / 10000;
        public static long NowSeconds => (Now.Ticks - startTimeTicks) / 10000000;

        public static string NowString { get { return Now.ToString(TimeStr); } }
        public static string NowString1 { get { return Now.ToString(TimeStr1); } }
        public static string NowStringWithSeconds { get { return Now.ToString(TimeStrWithSeconds); } }

        public static int GetTimeStamp(DateTime time)
        {
            return (int)(time - startTime).TotalSeconds;
        }

        public static DateTime GetDateTime(int timeStamp)
        {
            return startTime.AddSeconds(timeStamp);
        }
    }
}
