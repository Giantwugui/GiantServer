using System;

namespace Giant.Core
{
    public enum EventType
    {
        //1-1000 系统事件
        InitDone = 1,
        AffterFrontend = 2,
        AffterBackend = 3,
        CommandLine = 4,

        //1001-2000 战斗系统事件
        PositionChange = 1001,
    }

    public interface IEvent
    {
        void Run() { }
        void Run(object a) { }
        void Run(object a, object b) { }
        void Run(object a, object b, object c) { }
    }

    public abstract class Event : IEvent
    {
        public void Run()
        {
            Handle();
        }

        public abstract void Handle();
    }

    public abstract class Event<A> : IEvent
    {
        public void Run(object o)
        {
            Handle((A)o);
        }

        public new Type GetType() => typeof(A);

        public abstract void Handle(A a);
    }

    public abstract class Event<A, B> : IEvent
    {
        public void Run(object a, object b)
        {
            Handle((A)a, (B)b);
        }

        public new Type GetType() => typeof(A);

        public abstract void Handle(A a, B b);
    }

    public abstract class Event<A, B, C> : IEvent
    {
        public void Run(object a, object b, object c)
        {
            Handle((A)a, (B)b, (C)c);
        }

        public new Type GetType() => typeof(A);

        public abstract void Handle(A self, B a, C b);
    }
}
