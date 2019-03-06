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

            return (T)component;
        }
    }
}
