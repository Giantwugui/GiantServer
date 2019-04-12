using System;
using System.Collections.Generic;
using System.Text;

namespace Giant.Net
{
    public class TimeHelper
    {
        public static DateTime Now
        {
            get { return DateTime.UtcNow.ToLocalTime();  }
        }

        public static string NowString { get { return Now.ToString("yyyy-MM-dd hh:mm:ss"); } }
    }
}
