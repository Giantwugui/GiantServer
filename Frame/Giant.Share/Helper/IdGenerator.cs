using System;

namespace Giant.Share
{
    public class IdGenerator
    {
        private static int startId = -1;
        private static string timePrefix = "";
        private static uint uintIdStart = ((uint)1) << 31;

        /// <summary>
        /// 重启失效
        /// </summary>
        public static uint NewId
        {
            get { return ++uintIdStart; }
        }

        /// <summary>
        /// 年月日时分服务器id和自增id组合的19位id,
        /// 每秒能产生 1亿 个id 0 ~ 99999999，
        /// <para>不保证多程序重复性</para>
        /// <para>启动程序后，重复周期是 10 年,</para>
        /// <para>上一次启动和下一次启动之间重启周期是1分钟</para>
        /// </summary>
        public static long LongId
        {
            get
            {
                string datekey = DateTime.Now.ToString("yyyyMMddHHmmss").Substring(3);
                lock (timePrefix)
                {
                    if (!timePrefix.Equals(datekey))
                    {
                        timePrefix = datekey;
                        startId = -1;
                    }
                }

                return Convert.ToInt64(datekey + startId.ToString().PadLeft(8, '0'));
            }
        }
    }
}
