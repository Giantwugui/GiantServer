using Giant.Share;
using System;
using System.Collections.Generic;

namespace Giant.Model
{
    public class TimerComponentUpdateSystem : UpdateSystem<TimerComponent>
    {
        public override void Update(TimerComponent self)
        {
            self.Update();
        }
    }


    public class TimerComponent : Component
    {
        private SortedDictionary<long, Queue<Action>> timerCallBack = new SortedDictionary<long, Queue<Action>>();


        public void Update()
        {
            long now = TimeHelper.Now();

            List<long> overTime = new List<long>();

            foreach (var kv in timerCallBack)
            {
                if (kv.Key <= now)
                {
                    overTime.Add(kv.Key);
                }
            }

            foreach (var time in overTime)
            {
                if (timerCallBack.TryGetValue(time, out var queue))
                {
                    timerCallBack.Remove(time);

                    while (queue.TryDequeue(out var action))
                    {
                        action();
                    }
                }
            }
        }

        public void AddTimer(long time, Action action)
        {
            if (timerCallBack.ContainsKey(time))
            {

                timerCallBack[time].Enqueue(action);
            }
            else
            {
                timerCallBack.Add(time, new Queue<Action>());
                timerCallBack[time].Enqueue(action);
            }
        }
    }
}
