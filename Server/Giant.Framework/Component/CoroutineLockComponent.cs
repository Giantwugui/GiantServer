using Giant.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Giant.Framework
{
    public class CoroutineLock : IDisposable
    {
        private long Key { get; set; }

        public CoroutineLock(long key)
        {
            Key = key;
        }

        public void Dispose()
        {
            CoroutineLockComponent.Instance.Notify(Key);
        }
    }

    public class LockQueue : IDisposable
    {
        private readonly Queue<TaskCompletionSource<CoroutineLock>> queue = new Queue<TaskCompletionSource<CoroutineLock>>();

        public void Dispose()
        {
            queue.Clear();
        }

        public void Enqueue(TaskCompletionSource<CoroutineLock> tcs)
        {
            queue.Enqueue(tcs);
        }

        public TaskCompletionSource<CoroutineLock> Dequeue()
        {
            return queue.Dequeue();
        }

        public int Count
        {
            get
            {
                return queue.Count;
            }
        }
    }

    public class CoroutineLockComponent : Component
    {
        private readonly Dictionary<long, LockQueue> lockQueues = new Dictionary<long, LockQueue>();

        public static CoroutineLockComponent Instance { get; } = new CoroutineLockComponent();

        public override void Dispose()
        {
            foreach (var kv in lockQueues)
            {
                kv.Value.Dispose();
            }
            lockQueues.Clear();
        }

        public async Task<CoroutineLock> Wait(long key)
        {
            if (!lockQueues.TryGetValue(key, out LockQueue lockQueue))
            {
                lockQueues.Add(key, new LockQueue());
                return new CoroutineLock(key);
            }
            TaskCompletionSource<CoroutineLock> tcs = new TaskCompletionSource<CoroutineLock>();
            lockQueue.Enqueue(tcs);
            return await tcs.Task;
        }

        public void Notify(long key)
        {
            if (!lockQueues.TryGetValue(key, out LockQueue lockQueue))
            {
                return;
            }

            if (lockQueue.Count == 0)
            {
                lockQueues.Remove(key);
                lockQueue.Dispose();
                return;
            }

            TaskCompletionSource<CoroutineLock> tcs = lockQueue.Dequeue();
            tcs.SetResult(new CoroutineLock(key));
        }
    }
}