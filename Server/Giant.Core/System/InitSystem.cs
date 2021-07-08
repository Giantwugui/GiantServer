namespace Giant.Core
{
    public interface IInitSystem
    {
        void Init() { }
    }

    public interface IInitSystem<P1>
    {
        void Init(P1 p1);
    }

    public interface IInitSystem<P1, P2>
    {
        void Init(P1 p1, P2 p2);
    }

    public interface IInitSystem<P1, P2, P3>
    {
        void Init(P1 p1, P2 p2, P3 p3);
    }

    public interface IInitSystem<P1, P2, P3, P4>
    {
        void Init(P1 p1, P2 p2, P3 p3, P4 p4);
    }

    public interface IInitSystem<P1, P2, P3, P4, P5>
    {
        void Init(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5);
    }

    public interface IInitSystem<P1, P2, P3, P4, P5, P6>
    {
        void Init(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6);
    }

    [ObjectSystem]
    public abstract class InitSystem : Component, IInitSystem
    {
        public abstract void Init();
    }

    [ObjectSystem]
    public abstract class InitSystem<P> : Component, IInitSystem<P>
    {
        public abstract void Init(P t1);
    }

    [ObjectSystem]
    public abstract class InitSystem<P1, P2> : Component, IInitSystem<P1, P2>
    {
        public abstract void Init(P1 p1, P2 p2);
    }

    [ObjectSystem]
    public abstract class InitSystem<P1, P2, P3> : Component, IInitSystem<P1, P2, P3>
    {
        public abstract void Init(P1 p1, P2 p2, P3 p3);
    }

    [ObjectSystem]
    public abstract class InitLoadSystem : Component, IInitSystem, ILoadSystem
    {
        public void Init()
        {
            Load();
        }

        public abstract void Load();
    }

    [ObjectSystem]
    public abstract class InitUpdateSystem : Component, IInitSystem, IUpdateSystem
    {
        public abstract void Init();
        public abstract void Update(double dt);
    }
}
