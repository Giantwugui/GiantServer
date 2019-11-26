namespace Giant.Core
{
    partial class Component
    {
        public T GetComponent<T>() where T : Component, new()
        {
            componentes.TryGetValue(typeof(T), out var component);
            return component as T;
        }

        public T AddComponent<T>() where T : Component, IInitSystem, new()
        {
            T component = ComponentFactory.CreateComponent<T>();
            AddComponent(component);
            return component;
        }

        public T AddComponent<T, T1>(T1 t1) where T : Component, IInitSystem<T1>, new()
        {
            T component = ComponentFactory.CreateComponent<T, T1>(t1);
            AddComponent(component);
            return component;
        }

        public T AddComponent<T, T1, T2>(T1 t1, T2 t2) where T : Component, IInitSystem<T1, T2>, new()
        {
            T component = ComponentFactory.CreateComponent<T, T1, T2>(t1, t2);
            AddComponent(component);
            return component;
        }

        public T AddComponent<T, T1, T2, T3>(T1 t1, T2 t2, T3 t3) where T : Component, IInitSystem<T1, T2, T3>, new()
        {
            T component = ComponentFactory.CreateComponent<T, T1, T2, T3>(t1, t2, t3);
            AddComponent(component);
            return component;
        }

        public T AddComponent<T, T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4) where T : Component, IInitSystem<T1, T2, T3, T4>, new()
        {
            T component = ComponentFactory.CreateComponent<T, T1, T2, T3, T4>(t1, t2, t3, t4);
            AddComponent(component);
            return component;
        }

        public T AddComponent<T, T1, T2, T3, T4, T5>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) where T : Component, IInitSystem<T1, T2, T3, T4, T5>, new()
        {
            T component = ComponentFactory.CreateComponent<T, T1, T2, T3, T4, T5>(t1, t2, t3, t4, t5);
            AddComponent(component);
            return component;
        }

        private void AddComponent(Component component)
        {
            componentes.Add(component.GetType(), component);
            Scene.EventSystem.Add(component);

        }
    }
}
