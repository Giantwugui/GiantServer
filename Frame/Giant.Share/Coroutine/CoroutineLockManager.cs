using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Giant.Share
{
    public class CoroutineLock : IDisposable
    {
        private long Key { get; set; }

        public CoroutineLock(long key)
        {
            this.Key = key;
        }

        public void Dispose()
        {
            CoroutineLockManager.Instance.Notify(this.Key);
        }
    }

    public class LockQueue : IDisposable
    {
        private readonly Queue<TaskCompletionSource<CoroutineLock>> queue = new Queue<TaskCompletionSource<CoroutineLock>>();

        public void Dispose()
        {
            this.queue.Clear();
        }

        public void Enqueue(TaskCompletionSource<CoroutineLock> tcs)
        {
            this.queue.Enqueue(tcs);
        }

        public TaskCompletionSource<CoroutineLock> Dequeue()
        {
            return this.queue.Dequeue();
        }

        public int Count
        {
            get
            {
                return this.queue.Count;
            }
        }
    }

    public class CoroutineLockManager : IDisposable
    {
        private readonly Dictionary<long, LockQueue> lockQueues = new Dictionary<long, LockQueue>();

        public static CoroutineLockManager Instance { get; } = new CoroutineLockManager();

        public void Dispose()
        {        
            foreach (var kv in this.lockQueues)
            {
                kv.Value.Dispose();
            }
            this.lockQueues.Clear();
        }

        public async Task<CoroutineLock> Wait(long key)
        {
            if (!this.lockQueues.TryGetValue(key, out LockQueue lockQueue))
            {
                this.lockQueues.Add(key, new LockQueue());
                return new CoroutineLock(key);
            }
            TaskCompletionSource<CoroutineLock> tcs = new TaskCompletionSource<CoroutineLock>();
            lockQueue.Enqueue(tcs);
            return await tcs.Task;
        }
        
        public void Notify(long key)
        {
            if (!this.lockQueues.TryGetValue(key, out LockQueue lockQueue))
            {
                return;
            }
            
            if (lockQueue.Count == 0)
            {
                this.lockQueues.Remove(key);
                lockQueue.Dispose();
                return;
            }
            
            TaskCompletionSource<CoroutineLock> tcs = lockQueue.Dequeue();
            tcs.SetResult(new CoroutineLock(key));
        }
    }
}