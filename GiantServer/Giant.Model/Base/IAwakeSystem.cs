using System;
using System.Collections.Generic;
using System.Text;

namespace Giant.Model
{
    interface IAwake
    {
        Type GetType();
    }

    interface IAwakeSystem
    {
        void Run(object o);
    }

    interface IAwakeSystem<A>
    {
        void Run(object o, A a);
    }

    interface IAwakeSystem<A, B>
    {
        void Run(object o, A a, B b);
    }

    interface IAwakeSystem<A, B, C>
    {
        void Run(object o, A a, B b, C c);
    }

    public abstract class AwakeSystem<T> : IAwake, IAwakeSystem
    {
        public Type GetType()
        {
            return typeof(T);
        }


        public void Run(object o)
        {
            this.Awake((T)o);
        }

        public abstract void Awake(T t);
    }

    public abstract class AwakeSystem<T, A> : IAwake, IAwakeSystem<A>
    {
        public Type GetType()
        {
            return typeof(T);
        }

        public void Run(object o, A a)
        {
            this.Awake((T)o, a);
        }
        public abstract void Awake(T t, A a);
    }

    public abstract class AwakeSystem<T, A, B> : IAwake, IAwakeSystem<A, B>
    {
        public Type GetType()
        {
            return typeof(T);
        }

        public void Run(object o, A a, B b)
        {
            this.Awake((T)o, a, b);
        }

        public abstract void Awake(T t, A a, B b);
    }

    public abstract class AwakeSystem<T, A, B, C> : IAwake, IAwakeSystem<A, B, C>
    {
        public Type GetType()
        {
            return typeof(T);
        }

        public void Run(object o, A a, B b, C c)
        {
            this.Awake((T)o, a, b, c);
        }

        public abstract void Awake(T t, A a, B b, C c);
    }
}
