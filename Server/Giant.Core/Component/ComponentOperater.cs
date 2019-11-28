namespace Giant.Core
{
    partial class Component
    {
        public T AddComponentWithCreate<T>() where T : Component, IInitSystem, new()
        {
            T component = ComponentFactory.CreateComponent<T>();
            AddComponent(component);
            return component;
        }

        public T AddComponentWithCreate<T, T1>(T1 t1) where T : Component, IInitSystem<T1>, new()
        {
            T component = ComponentFactory.CreateComponent<T, T1>(t1);
            AddComponent(component);
            return component;
        }

        public T AddComponentWithCreate<T, T1, T2>(T1 t1, T2 t2) where T : Component, IInitSystem<T1, T2>, new()
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
            componentes.Add(component.GetType(), component);
            Scene.EventSystem.Add(component);
        }
    }
}
