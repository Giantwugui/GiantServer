using Giant.Share;
using System;
using System.Collections.Generic;

namespace Giant.Frame
{
    public class TimerInfo
    {
        public long Id;
        public long Time;
        public Action CallBack;
    }

    public class Timer
    {
        private long timerId = 0;
        private long MinTime = 0;//最近过期时间
        private Dictionary<long, TimerInfo> timers = new Dictionary<long, TimerInfo>();//timerid,timerinfo
        private readonly SortedDictionary<long, List<long>> waitDicts = new SortedDictionary<long, List<long>>();//time, timerId

        public Queue<long> outOfTime = new Queue<long>();
        public Queue<long> outOfTimeIds = new Queue<long>();


        public void Update()
        {
            long now = TimeHelper.NowMilliSeconds;

            foreach(var kv in waitDicts)
            {
                if (kv.Key > now)
                {
                    MinTime = kv.Key;
                    break;
                }
                else
                {
                    outOfTime.Enqueue(kv.Key);

                    waitDicts[kv.Key].ForEach(x => outOfTimeIds.Enqueue(x));
                    waitDicts[kv.Key].Clear();
                }
            }

            while (outOfTime.TryDequeue(out long time))
            {
                waitDicts.Remove(time);
            }

            while (outOfTimeIds.TryDequeue(out long timerId))
            {
                if (timers.TryGetValue(timerId, out TimerInfo timerInfo))
                {
                    timerInfo.CallBack();
                    timers.Remove(timerId);
                }
            }
        }

        public void Wait(long delay, Action callBack)
        {
            TimerInfo timerInfo = new TimerInfo() { Id = ++timerId, Time = TimeHelper.NowMilliSeconds + delay, CallBack = callBack };

            Add(timerInfo);
        }

        public void WaitTill(long time, Action callBack)
        {
            TimerInfo timerInfo = new TimerInfo() { Id = ++timerId, Time = time, CallBack = callBack };

            Add(timerInfo);
        }


        private void Add(TimerInfo timerInfo)
        {
            if (timerInfo.Time < MinTime)
            {
                MinTime = timerInfo.Time;
            }

            timers[timerInfo.Id] = timerInfo;

            if (!waitDicts.ContainsKey(timerInfo.Time))
            {
                waitDicts.Add(timerInfo.Time, new List<long>());
            }

            waitDicts[timerInfo.Time].Add(timerInfo.Id);
        }

    }
}
