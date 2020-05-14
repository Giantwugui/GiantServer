using System;
using System.Collections.Generic;

namespace Giant.Core
{
    public abstract class SingleDataComponent<T, M> : SingleComponent<T>, ILoadSystem where M : IData<M> where T : Component, new()
    {
        private readonly Dictionary<int, M> models = new Dictionary<int, M>();
        public Dictionary<int, M> Models => models;

        public override void Init()
        {
            Clear();

            Load();
        }

        public M GetModel(int id)
        {
            models.TryGetValue(id, out var mode);
            return mode;
        }

        public void AddModel(M model)
        {
            models.Add(model.Id, model);
        }

        public virtual void Clear()
        {
            models.Clear();
        }


        public abstract void Load();

        public virtual void Load(string fileName)
        {
            Clear();
            var datas = DataComponent.Instance.GetDatas(fileName);
            if (datas == null)
            {
                throw new Exception($"have no xml file {fileName}");
            }

            M model;
            foreach (var kv in datas)
            {
                model = Activator.CreateInstance<M>();
                model.Bind(kv.Value);

                AddModel(model);
            }
        }
    }
}
