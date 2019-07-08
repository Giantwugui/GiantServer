using System;
using System.Threading.Tasks;
using Giant.Log;

namespace Giant.DataTask
{
    public abstract class DataTask : IDataTask
    {
        public long TaskId { get; set; }

        public abstract IDataService DataService { get; }

        public abstract Task Run();
    }


    public abstract class DataTask<T> : DataTask
    {
        private TaskCompletionSource<T> Tcs { get; set; }

        public virtual Task<T> Task()
        {
            this.Tcs = new TaskCompletionSource<T>();

            this.AddToTaskPool();

            return this.Tcs.Task;
        }

        public virtual void SetResult(T result)
        {
            this.Tcs.SetResult(result);
        }

        public virtual void SetException(Exception ex)
        {
            this.Tcs.SetException(ex);

            Logger.Error(ex);
        }

        private void AddToTaskPool()
        {
            this.DataService.TaskPool.AddTask(this);
        }
    }
}
