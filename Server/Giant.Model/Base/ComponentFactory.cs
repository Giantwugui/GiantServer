using System;

namespace Giant.Model
{
    /// <summary>
    /// 组件构造工厂
    /// </summary>
    public static class ComponentFactory
    {
        public static T Create<T>(bool fromPool = true) where T : Component
        {
            Component component;

            if (fromPool)
            {
                component = Game.ObjectPool.Fatch<T>();
            }
            else
            {
                component = Activator.CreateInstance<T>();
            }

            return (T)component;
        }

        public static T CreateWithParent<T>(Component parent, bool fromPool = true) where T : Component
        {
            Component component;

            if (fromPool)
            {
                component = Game.ObjectPool.Fatch<T>();
            }
            else
            {
                component = Activator.CreateInstance<T>();
            }

            component.Parent = parent;

            Game.EventSystem.Awake(component);

            return (T)component;
        }

        public static T CreateWithParent<T, A>(Component parent, A a, bool fromPool = true) where T : Component
        {
            Component component;

            if (fromPool)
            {
                component = Game.ObjectPool.Fatch<T>();
            }
            else
            {
                component = Activator.CreateInstance<T>();
            }

            component.Parent = parent;

            Game.EventSystem.Awake(component, a);

            return (T)component;
        }

        public static T CreateWithParent<T, A, B>(Component parent, A a, B b, bool fromPool = true) where T : Component
        {
            Component component;

            if (fromPool)
            {
                component = Game.ObjectPool.Fatch<T>();
            }
            else
            {
                component = Activator.CreateInstance<T>();
            }

            component.Parent = parent;

            Game.EventSystem.Awake(component, a,  b);

            return (T)component;
        }

        public static T CreateWithParent<T, A, B, C>(Component parent, A a, B b, C c, bool fromPool = true) where T : Component
        {
            Component component;

            if (fromPool)
            {
                component = Game.ObjectPool.Fatch<T>();
            }
            else
            {
                component = Activator.CreateInstance<T>();
            }

            component.Parent = parent;

            Game.EventSystem.Awake(component, a, b, c);

            return (T)component;
        }
    }
}
