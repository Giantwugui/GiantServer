using System;
using System.Collections.Generic;
using System.Text;

namespace Giant.Model
{
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
    }
}
