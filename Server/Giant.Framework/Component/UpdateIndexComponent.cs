using Giant.Core;
using System;
using System.Collections.Generic;

namespace Giant.Framework
{
    public class UpdateIndexComponent : InitSystem, ILoadSystem
    {
        public List<DataTask> taskList = new List<DataTask>();

        public override void Init()
        {
            Load();
        }

        public void Load()
        {
            taskList.Clear();
            List<Type> types = Scene.EventSystem.GetTypes(typeof(MongoDBIndexAttribute));
            foreach (var kv in types)
            {
                if (Activator.CreateInstance(kv) is DataTask task)
                {
                    taskList.Add(task);
                }
            }
            taskList.ForEach(async x => await x.Run());
        }
    }
}
