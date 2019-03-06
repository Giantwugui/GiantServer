using System;
using System.Collections.Generic;

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

        /// <summary>
        /// 回收
        /// </summary>
        /// <param name="component"></param>
        public void Recycle(Component component)
        {
            Type type = component.GetType();

            if (dictionary.TryGetValue(type, out Queue<Component> components))
            {
                if (components.Count < 100)
                {
                    dictionary[type].Enqueue(component);
                }
            }
        }
    }
}
