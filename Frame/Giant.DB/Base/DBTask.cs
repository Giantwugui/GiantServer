using System;
using System.Threading.Tasks;
using Giant.Log;

namespace Giant.DB
{
    public abstract class DBTask
    {
        public long TaskId { get; set; }

        public DBService DBService { get; set; }

        public abstract Task Run();
    }

    public abstract class DBTask<T> : DBTask
    {
        public TaskCompletionSource<T> Tcs { get; set; }

        public virtual Task<T> Task()
        {
            this.Tcs = new TaskCompletionSource<T>();

            this.AddToTaskManager();

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

        private void AddToTaskManager()
        {
            this.DBService.TaskManager.AddTask(this);
        }
    }
}
