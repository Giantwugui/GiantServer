using System;

namespace Giant.Core
{
    partial class Component
    {
        public T AddComponent<T>() where T : Component, new()
        {
            T component = ComponentFactory.CreateComponent<T>();
            AddComponent(component);
            return component;
        }

        public T AddComponent<T, T1>(T1 t1) where T : Component, new()
        {
            T component = ComponentFactory.CreateComponent<T, T1>(t1);
            AddComponent(component);
            return component;
        }

        public T AddComponent<T, T1, T2>(T1 t1, T2 t2) where T : Component, new()
        {
            T component = ComponentFactory.CreateComponent<T, T1, T2>(t1, t2);
            AddComponent(component);
            return component;
        }

        public T GetComponent<T>() where T : Component, new()
        {
            componentes.TryGetValue(typeof(T), out var component);
            return component as T;
        }

        public void AddComponent(Component component)
        {
            Type type = component.GetType();
            if (componentes.ContainsKey(type))
            {
                throw new Exception($"add repeated component {type}");
            }

            componentes.Add(type, component);

            Scene.EventSystem.Regist(component);
        }
    }
}
