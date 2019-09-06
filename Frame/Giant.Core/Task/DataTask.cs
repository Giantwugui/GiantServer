using System;
using System.Threading.Tasks;

namespace Giant.Core
{
    public abstract class DataTask : IDataTask
    {
        public long TaskId { get; set; }

        public abstract IDataService DataService { get; }

        public abstract Task Run();

        public abstract void SetException(Exception ex);
    }


    public abstract class DataTask<T> : DataTask
    {
        private TaskCompletionSource<T> Tcs { get; set; }

        public Task<T> Task()
        {
            this.Tcs = new TaskCompletionSource<T>();

            this.AddToTaskPool();

            return this.Tcs.Task;
        }

        public void SetResult(T result)
        {
            this.Tcs.SetResult(result);
        }

        public override void SetException(Exception ex)
        {
            this.Tcs.SetException(ex);
        }

        private void AddToTaskPool()
        {
            this.DataService.TaskPool.AddTask(this);
        }
    }
}
