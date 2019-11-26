using System.Collections.Generic;

namespace Giant.Core
{
    public class TaskPool
    {
        private long taskId;
        private readonly List<TaskQueue> taskList = new List<TaskQueue>();

        public TaskPool(int taskCount)
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

        public void AddTask(DataTask task)
        {
            task.TaskId = ++taskId;
            taskList[(int)(taskId % taskList.Count)].Add(task);
        }
    }
}
