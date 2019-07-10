using Giant.Log;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Giant.DataTask
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
                try
                {
                    DataTask task = await Get();

                    try
                    {
                        await task.Run();
                    }
                    catch (Exception ex)
                    {
                        task.SetException(ex);
                        Logger.Error(ex);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }

            }
        }

        public void Add(DataTask task)
        {
            //有等待获取任务的任务，则直接将该获取任务的任务设置为完成状态
            if (this.tcs != null && !this.tcs.Task.IsCompleted)
            {
                var willFinish = this.tcs;
                this.tcs = null;
                willFinish.SetResult(task);
                return;
            }
            else
            {
                this.tasks.Enqueue(task);
            }
        }

        private Task<DataTask> Get()
        {
            this.tcs = new TaskCompletionSource<DataTask>();
            if (this.tasks.TryDequeue(out var task))
            {
                this.tcs.SetResult(task);
            }
            return this.tcs.Task;
        }
    }
}
