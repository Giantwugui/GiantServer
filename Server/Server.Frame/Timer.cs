using Giant.Share;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Frame
{
    public class TimerInfo
    {
        public long Id { get; private set; }
        public long Time { get; private set; }
        public Action Action { get; private set; }

        public TimerInfo(long id, long time, Action action)
        {
            this.Id = id;
            this.Time = time;
            this.Action = action;
        }
    }

    public class Timer
    {
        private long minTime = 0;//最近过期时间
        private readonly Dictionary<long, TimerInfo> timers = new Dictionary<long, TimerInfo>();//timerid,timerinfo
        private readonly SortedDictionary<long, List<long>> waitDicts = new SortedDictionary<long, List<long>>();//time, timerId

        private readonly Queue<long> outOfTime = new Queue<long>();
        private readonly Queue<long> outOfTimeIds = new Queue<long>();

        private long timerId = 0;
        public long TimerId => ++timerId;

        public static Timer Instance { get; } = new Timer();

        private Timer() { }



        public void Update()
        {
            long now = TimeHelper.NowMilliSeconds;

            foreach(var kv in waitDicts)
            {
                if (kv.Key > now)
                {
                    minTime = kv.Key;
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
                    timerInfo.Action();
                    timers.Remove(timerId);
                }
            }
        }

        public void Wait(long delay, Action action)
        {
            TimerInfo timerInfo = new TimerInfo(TimerId, TimeHelper.NowMilliSeconds + delay, action);

            Add(timerInfo);
        }

        public void WaitTill(long time, Action callBack)
        {
            TimerInfo timerInfo = new TimerInfo(TimerId, time, callBack);

            Add(timerInfo);
        }

        public Task<bool> WaitAsync(int delay)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            TimerInfo info = new TimerInfo(TimerId, TimeHelper.NowMilliSeconds + delay, () => tcs.SetResult(true));
            Add(info);

            return tcs.Task;
        }


        private void Add(TimerInfo timerInfo)
        {
            if (timerInfo.Time < minTime)
            {
                minTime = timerInfo.Time;
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
