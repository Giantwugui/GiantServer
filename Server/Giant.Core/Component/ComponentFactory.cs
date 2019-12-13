using System;

namespace Giant.Core
{
    public class ComponentFactory
    {
        public static T CreateComponent<T>() where T : Component
        {
            T component = Activator.CreateInstance<T>();

            IInitSystem system = component as IInitSystem;
            system?.Init();

            return component;
        }

        public static T CreateComponent<T, P>(P p) where T : Component
        {
            T component = Activator.CreateInstance<T>();

            IInitSystem<P> system = component as IInitSystem<P>;
            system?.Init(p);

            return component;
        }

        public static T CreateComponent<T, P1, P2>(P1 tp1, P2 p2) where T : Component
        {
            T component = Activator.CreateInstance<T>();

            IInitSystem<P1, P2> system = component as IInitSystem<P1, P2>;
            system?.Init(tp1, p2);

            return component;
        }

        public static T CreateComponent<T, P1, P2, P3>(P1 p1, P2 p2, P3 p3) where T : Component
        {
            T component = Activator.CreateInstance<T>();

            IInitSystem<P1, P2, P3> system = component as IInitSystem<P1, P2, P3>;
            system?.Init(p1, p2, p3);

            return component;
        }

        public static T CreateComponent<T, P1, P2, P3, P4>(P1 p1, P2 p2, P3 p3, P4 p4) where T : Component
        {
            T component = Activator.CreateInstance<T>();

            IInitSystem<P1, P2, P3, P4> system = component as IInitSystem<P1, P2, P3, P4>;
            system?.Init(p1, p2, p3, p4);

            return component;
        }

        public static T CreateComponentWithParent<T>(Component parent) where T : Component
        {
            T component = Activator.CreateInstance<T>();
            component.Parent = parent;

            IInitSystem system = component as IInitSystem;
            system?.Init();

            return component;
        }

        public static T CreateComponentWithParent<T, P1>(Component parent, P1 p1) where T : Component
        {
            T component = Activator.CreateInstance<T>();
            component.Parent = parent;

            IInitSystem<P1> system = component as IInitSystem<P1>;
            system?.Init(p1);

            return component;
        }

        public static T CreateComponentWithParent<T, P1, P2>(Component parent, P1 p1, P2 p2) where T : Component
        {
            T component = Activator.CreateInstance<T>();
            component.Parent = parent;

            IInitSystem<P1, P2> system = component as IInitSystem<P1, P2>;
            system?.Init(p1, p2);

            return component;
        }
    }
}
