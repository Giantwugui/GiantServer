using Giant.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Giant.Framework
{
    public interface ITimer
    {
        Action Action { get; }
        public void Run();
    }

    public class OnceTimer : InitSystem<Action>, ITimer
    {
        public Action Action { get; private set; }

        public override void Init(Action action)
        {
            Action = action;
        }

        public void Run()
        {
            try
            {
                Action?.Invoke();
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex);
            }
            finally
            {
                GetParent<TimerComponent>().Remove(InstanceId);
            }
        }
    }

    public class RepeatTimer : InitSystem<long, Action>, ITimer
    {
        public long RepeatTime { get; private set; }
        public Action Action { get; private set; }
        public int RepeatedCount { get; private set; }

        public override void Init(long repeatTime, Action action)
        {
            RepeatTime = repeatTime;
            Action = action;
        }

        public void Run()
        {
            ++RepeatedCount;

            long time = TimeHelper.NowMilliSeconds + RepeatTime;
            TimerComponent component = GetParent<TimerComponent>();
            component.Add(time, InstanceId);

            try
            {
                Action?.Invoke();
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex);
            }
        }
    }

    public class TimerComponent : InitSystem, IUpdateSystem
    {
        private long minTime = 0;//最近过期时间
        private readonly Dictionary<long, ITimer> timers = new Dictionary<long, ITimer>();//timerid,timerinfo
        private readonly SortedDictionary<long, List<long>> waitDicts = new SortedDictionary<long, List<long>>();//time, timerId

        private readonly Queue<long> outOfTime = new Queue<long>();
        private readonly Queue<long> outOfTimeIds = new Queue<long>();

        public static TimerComponent Instance { get; private set; }

        public override void Init()
        {
            Instance = this;
        }

        public void Update(double dt)
        {
            long now = TimeHelper.NowMilliSeconds;

            foreach (var kv in waitDicts)
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
                if (timers.TryGetValue(timerId, out ITimer timer))
                {
                    try
                    {
                        timer.Run();
                    }
                    catch (Exception ex)
                    {
                        Logger.Log.Error(ex);
                    }
                }
            }
        }

        public void Wait(long delay, Action action)
        {
            OnceTimer timer = ComponentFactory.CreateComponent<OnceTimer, Action>(action);
            Add(TimeHelper.NowMilliSeconds + delay, timer.InstanceId, timer);
        }

        public void WaitTill(long time, Action action)
        {
            OnceTimer timer = ComponentFactory.CreateComponentWithParent<OnceTimer, Action>(this, action);
            Add(time, timer.InstanceId, timer);
        }

        public Task<bool> WaitAsync(int delay)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            OnceTimer timer = ComponentFactory.CreateComponentWithParent<OnceTimer, Action>(this, () => tcs.SetResult(true));
            Add(TimeHelper.NowMilliSeconds + delay, timer.InstanceId, timer);

            return tcs.Task;
        }

        public RepeatTimer AddRepeatTimer(long repeatTime, Action action)
        {
            RepeatTimer timer = ComponentFactory.CreateComponentWithParent<RepeatTimer, long, Action>(this, repeatTime, action);
            Add(TimeHelper.NowMilliSeconds + repeatTime, timer.InstanceId, timer);
            return timer;
        }

        public void Remove(long id)
        {
            if (id == 0) return;

            if (!timers.TryGetValue(id, out var timer))
            {
                return;
            }

            timers.Remove(id);
            (timer as Component).Dispose();
        }

        public void Add(long time, long id, ITimer timer)
        {
            timers[id] = timer;
            Add(time, id);
        }

        public void Add(long time, long id)
        {
            if (time < minTime)
            {
                minTime = time;
            }

            if (!waitDicts.ContainsKey(time))
            {
                waitDicts.Add(time, new List<long>());
            }

            waitDicts[time].Add(id);
        }
    }
}
