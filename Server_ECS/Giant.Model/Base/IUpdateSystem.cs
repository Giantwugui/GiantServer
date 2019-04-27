using System;
using System.Collections.Generic;
using System.Text;

namespace Giant.Model
{
    public interface IUpdateSystem
    {
        Type Type();

        void Run(object o);
    }

    public abstract class UpdateSystem<T> : IUpdateSystem
    {
        public Type Type()
        {
            return typeof(T);
        }

        public void Run(object o)
        {
            Update((T)o);
        }

        public abstract void Update(T self);
    }
}
