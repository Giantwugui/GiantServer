using System;
using System.Threading.Tasks;

namespace Giant.Core
{
    public abstract class DataTask : IDataTask
    {
        public long TaskId { get; set; }
        public abstract IDataService DataService { get; }

        public DataTask() { }
        public abstract Task Run();
        public abstract void SetException(Exception ex);
    }


    public abstract class DataTask<T> : DataTask
    {
        private TaskCompletionSource<T> tcs;

        /// <summary>
        /// 添加到任务队列，不需要返回结果
        /// </summary>
        public void Call()
        { 
            AddToTaskPool();
        }

        /// <summary>
        /// 添加到任务队列，处理后返回结果
        /// </summary>
        /// <returns></returns>
        public Task<T> Task()
        {
            tcs = new TaskCompletionSource<T>();

            AddToTaskPool();

            return tcs.Task;
        }

        /// <summary>
        /// 设置任务结果
        /// </summary>
        /// <param name="result">结果</param>
        public void SetResult(T result)
        {
            tcs?.SetResult(result);
        }

        /// <summary>
        /// 设置异常处理结果
        /// </summary>
        /// <param name="ex"></param>
        public override void SetException(Exception ex)
        {
            tcs?.SetException(ex);
        }

        /// <summary>
        /// 添加到任务队列
        /// </summary>
        private void AddToTaskPool()
        {
            DataService.TaskPool.AddTask(this);
        }
    }
}
