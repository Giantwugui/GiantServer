using System;
using System.Collections.Generic;
using System.Reflection;

namespace Giant.Model
{
    public enum DLLType
    {
        Model,
        Hotfix
    }

    /// <summary>
    /// 驱动系统
    /// </summary>
    public sealed class EventSystem
    {

        public Dictionary<long, Component> components = new Dictionary<long, Component>();

        private Dictionary<DLLType, Assembly> assemblies = new Dictionary<DLLType, Assembly>();

        private MulitMap<Type, Type> types = new MulitMap<Type, Type>();

        private MulitMap<Type, IAwakeSystem> awakeSystems = new MulitMap<Type, IAwakeSystem>();

        private MulitMap<Type, IUpdateSystem> updateSystems = new MulitMap<Type, IUpdateSystem>();

        private MulitMap<Type, ILoadSystem> loadSystems = new MulitMap<Type, ILoadSystem>();


        public void Add(DLLType dllType, Assembly assembly)
        {
            this.assemblies[dllType] = assembly;

            types.Clear();


            foreach (Assembly value in assemblies.Values)
            {
                foreach (Type type in value.GetTypes())
                {
                    object[] attributes = type.GetCustomAttributes(typeof(BaseAttribute), false);

                    if (attributes.Length == 0) continue;

                    BaseAttribute attribute = attributes[0] as BaseAttribute;

                    types.Add(attribute.AttributeType, type);
                }
            } 

            awakeSystems.Clear();
            updateSystems.Clear();
            loadSystems.Clear();

            foreach (Type type in types.Get(typeof(ObjectSystemAttribute)))
            {
                object[] attributes = type.GetCustomAttributes(typeof(ObjectSystemAttribute), false);

                if (attributes.Length == 0) continue;

                object _object = Activator.CreateInstance(type);

                switch (_object)
                {
                    case IAwakeSystem awakeSystem:
                        awakeSystems.Add(awakeSystem.Type(), awakeSystem);
                        break;
                    case IUpdateSystem updateSystem:
                        updateSystems.Add(updateSystem.Type(), updateSystem);
                        break;
                    case ILoadSystem loadSystem:
                        loadSystems.Add(loadSystem.Type(), loadSystem);
                        break;
                }
            }


        }


        public void AddComponent(Component component)
        {
        }


        public List<Type> GetTypes(Type  type)
        {
            if (this.types.ContainsKey(type))
            {
                return this.types[type];
            }

            return new List<Type>() ;
        }


        public void Awake<T>(T component) where T : Component
        {
            Type type = component.GetType();

            List<IAwakeSystem> list = awakeSystems[type];

            foreach (IAwakeSystem awakeSystem in list)
            {
                IAwake awake = awakeSystem as IAwake;

                awake.Run(component);
            }
        }

        public void Awake<T, A>(T component, A a) where T : Component
        {
            Type type = component.GetType();

            List<IAwakeSystem> list = awakeSystems[type];

            foreach (IAwakeSystem awakeSystem in list)
            {
                IAwake<A> awake = awakeSystem as IAwake<A>;

                awake.Run(component, a);
            }
        }

        public void Awake<T, A, B>(T component, A a, B b) where T : Component
        {
            Type type = component.GetType();

            List<IAwakeSystem> list = awakeSystems[type];

            foreach (IAwakeSystem awakeSystem in list)
            {
                IAwake<A, B> awake = awakeSystem as IAwake<A, B>;

                awake.Run(component, a, b);
            }
        }

        public void Awake<T, A, B, C>(T component, A a, B b, C c) where T : Component
        {
            Type type = component.GetType();

            List<IAwakeSystem> list = awakeSystems[type];

            foreach (IAwakeSystem awakeSystem in list)
            {
                IAwake<A, B, C> awake = awakeSystem as IAwake<A, B, C>;

                awake.Run(component, a, b, c);
            }
        }


        public void Update()
        {

        }
    }
}
