using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Giant.Core
{
    class TaskQueue
    {
        private TaskCompletionSource<DataTask> tcs;
        private readonly Queue<DataTask> tasks = new Queue<DataTask>();

        public int TaskCount => tasks.Count;

        public async void Start()
        {
            while (true)
            {
                DataTask task = await Get();

                try
                {
                    await task.Run();
                }
                catch (Exception ex)
                {
                    task.SetException(ex);
                }
            }
        }

        public void Add(DataTask task)
        {
            //有等待获取任务的任务，则直接将该获取任务的任务设置为完成状态
            if (tcs != null && !tcs.Task.IsCompleted)
            {
                var willFinish = tcs;
                tcs = null;
                willFinish.SetResult(task);
                return;
            }
            else
            {
                tasks.Enqueue(task);
            }
        }

        private Task<DataTask> Get()
        {
            tcs = new TaskCompletionSource<DataTask>();
            if (tasks.TryDequeue(out var task))
            {
                tcs.SetResult(task);
            }
            return tcs.Task;
        }
    }
}
