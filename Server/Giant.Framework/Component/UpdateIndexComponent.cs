using Giant.Core;
using System;
using System.Collections.Generic;

namespace Giant.Framework
{
    public class UpdateIndexComponent : InitSystem, ILoadSystem
    {
        public List<DataTask<string>> taskList = new List<DataTask<string>>();

        public override void Init()
        {
            Load();
        }

        public void Load()
        {
            taskList.Clear();
            List<Type> types = Scene.EventSystem.GetTypes(typeof(MongoDBIndexAttribute));
            if (types == null) return;

            foreach (var kv in types)
            {
                if (Activator.CreateInstance(kv) is DataTask<string> task)
                {
                    taskList.Add(task);
                }
            }
            taskList.ForEach(x => x.Call());
        }
    }
}
