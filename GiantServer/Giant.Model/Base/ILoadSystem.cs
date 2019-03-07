using System;
using System.Collections.Generic;
using System.Text;

namespace Giant.Model
{
    public interface ILoadSystem
    {
        Type Type();
    }

    public abstract class LoadSystem<T> : ILoadSystem
    {
        public Type Type()
        {
            return typeof(T);
        }
        public abstract void Load(T self);
    }
}
