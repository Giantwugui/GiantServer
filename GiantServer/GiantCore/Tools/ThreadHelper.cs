using System;
using System.Collections.Generic;
using System.Threading;

namespace GiantCore
{
    public class ThreadHelper
    {
        /// <summary>
        /// 创建线程
        /// </summary>
        public static Thread CreateThread(ThreadStart callBack, string threadName, bool autoRun = true)
        {
            Thread tempThread = new Thread(callBack)
            {
                Name = threadName,
                IsBackground = true
            };

            if (autoRun)
            {
                tempThread.Start();
            }

            return tempThread;
        }

        /// <summary>
        /// 在线程池中创建线程
        /// </summary>
        public static void CreateThreadInThreadPool(WaitCallback callBack, Dictionary<string, string> param)
        {
            ThreadPool.QueueUserWorkItem(callBack, param);
        }
    }
}
