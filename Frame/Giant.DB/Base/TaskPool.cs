using System.Collections.Generic;

namespace Giant.DB
{
    public class DBTaskPool
    {
        private long taskId;
        private readonly List<TaskQueue> taskList = new List<TaskQueue>();

        public DBService DBService { get; private set; }

        public DBTaskPool(int taskCount)
        {
            for (int i = 0; i < taskCount; ++i)
            {
                taskList.Add(new TaskQueue());
            }
        }

        public void Start()
        {
            taskList.ForEach(taskQueue => taskQueue.Start());
        }

        public void AddTask(DBTask task)
        {
            ++this.taskId;
            task.TaskId = this.taskId;
            taskList[(int)(this.taskId % this.taskList.Count)].Add(task);
        }


    }
}
