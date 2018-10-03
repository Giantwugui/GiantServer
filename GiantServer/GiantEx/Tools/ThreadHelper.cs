using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GiantCore.Tools
{
    public class ThreadHelper
    {
        /// <summary>
        /// 创建线程
        /// </summary>
        public static Thread CreateThread(ThreadStart callBack, string threadName, bool autoRun = true)
        {
            Thread tempThread = new Thread(callBack);
            tempThread.Name = threadName;
            tempThread.IsBackground = true;

            if (autoRun)
            {
                tempThread.Start();
            }

            return tempThread;
        }
    }
}
