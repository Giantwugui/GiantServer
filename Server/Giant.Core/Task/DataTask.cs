using System;
using System.Threading.Tasks;

namespace Giant.Core
{
    public abstract class DataTask : IDataTask
    {
        public DataTask() { }
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
            Tcs = new TaskCompletionSource<T>();

            AddToTaskPool();

            return Tcs.Task;
        }

        public void SetResult(T result)
        {
            Tcs.SetResult(result);
        }

        public override void SetException(Exception ex)
        {
            Tcs.SetException(ex);
        }

        private void AddToTaskPool()
        {
            DataService.TaskPool.AddTask(this);
        }
    }
}
