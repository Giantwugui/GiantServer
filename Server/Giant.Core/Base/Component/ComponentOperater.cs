using System;

namespace Giant.Core
{
    partial class Component
    {
        public T AddComponent<T>() where T : Component, new()
        {
            T component = ComponentFactory.Create<T>();
            AddComponent(component);
            return component;
        }

        public T AddComponent<T, T1>(T1 t1) where T : Component, new()
        {
            T component = ComponentFactory.Create<T, T1>(t1);
            AddComponent(component);
            return component;
        }

        public T AddComponent<T, T1, T2>(T1 t1, T2 t2) where T : Component, new()
        {
            T component = ComponentFactory.Create<T, T1, T2>(t1, t2);
            AddComponent(component);
            return component;
        }

        public T AddComponentWithParent<T>() where T : Component, new()
        {
            T component = ComponentFactory.CreateWithParent<T>(this);
            AddComponent(component);
            return component;
        }

        public T AddComponentWithParent<T>(Component parent) where T : Component, new()
        {
            T component = ComponentFactory.CreateWithParent<T>(parent);
            AddComponent(component);
            return component;
        }

        public T AddComponentWithParent<T, T1>(T1 t1) where T : Component, new()
        {
            T component = ComponentFactory.CreateWithParent<T, T1>(this, t1);
            AddComponent(component);
            return component;
        }

        public T AddComponentWithParent<T, T1>(Component parent, T1 t1) where T : Component, new()
        {
            T component = ComponentFactory.CreateWithParent<T, T1>(parent, t1);
            AddComponent(component);
            return component;
        }

        public T AddComponentWithParent<T, T1, T2>(T1 t1, T2 t2) where T : Component, new()
        {
            T component = ComponentFactory.CreateWithParent<T, T1, T2>(this, t1, t2);
            AddComponent(component);
            return component;
        }

        public T AddComponentWithParent<T, T1, T2>(Component parent, T1 t1, T2 t2) where T : Component, new()
        {
            T component = ComponentFactory.CreateWithParent<T, T1, T2>(parent, t1, t2);
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

            //注册Event load
            Scene.EventSystem.Register(component);
        }

        public void RemoveComponent<T>() where T : Component
        {
            Type type = typeof(T);
            if (componentes.TryGetValue(type, out var com))
            {
                componentes.Remove(type);

                if (!com.IsDisposed)
                {
                    com.Dispose();
                }
            }
        }
    }
}
