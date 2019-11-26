namespace Giant.Core
{
    public interface IInitSystem
    {
        void Init();
    }

    public interface IInitSystem<T1>
    {
        void Init(T1 t1);
    }

    public interface IInitSystem<T1, T2>
    {
        void Init(T1 t1, T2 t2);
    }

    public interface IInitSystem<T1, T2, T3>
    {
        void Init(T1 t1, T2 t2, T3 t3);
    }

    public interface IInitSystem<T1, T2, T3, T4>
    {
        void Init(T1 t1, T2 t2, T3 t3, T4 t4);
    }

    public interface IInitSystem<T1, T2, T3, T4, T5>
    {
        void Init(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5);
    }

    public interface IInitSystem<T1, T2, T3, T4, T5, T6>
    {
        void Init(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6);
    }

    public abstract class InitSystem<T> : IInitSystem<T>
    {
        public abstract void Init(T t1);
    }

    public abstract class InitSystem<T1, T2> : IInitSystem<T1, T2>
    {
        public abstract void Init(T1 t1, T2 t2);
    }

    public abstract class InitSystem<T1, T2, T3> : IInitSystem<T1, T2, T3>
    {
        public abstract void Init(T1 t1, T2 t2, T3 t3);
    }

    public abstract class InitSystem<T1, T2, T3, T4> : IInitSystem<T1, T2, T3, T4>
    {
        public abstract void Init(T1 t1, T2 t2, T3 t3, T4 t4);
    }

    public abstract class InitSystem<T1, T2, T3, T4, T5> : IInitSystem<T1, T2, T3, T4, T5>
    {
        public abstract void Init(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5);
    }
}
