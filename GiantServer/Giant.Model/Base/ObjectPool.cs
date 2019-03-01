using System;
using System.Collections.Generic;
using System.Text;

namespace Giant.Model
{
    public class ObjectPool : Component
    {
        private Dictionary<Type, Queue<Component>> dictionary = new Dictionary<Type, Queue<Component>>();

        public T Fatch<T>() where T : Component
        {
            Type type = typeof(T);

            return (T)this.Fatch(type);
        }

        public Component Fatch(Type type)
        {
            Component component;

            if (!dictionary.TryGetValue(type, out var components))
            {
                component = (Component)Activator.CreateInstance(type);
            }
            else if (components.Count == 0)
            {
                component = (Component)Activator.CreateInstance(type);
            }
            else
            {
                component = components.Dequeue();
            }

            return component;
        }
    }
}
