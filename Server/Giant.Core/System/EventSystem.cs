using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Giant.Core
{
    public class EventSystem
    {
        private readonly Dictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>();
        private readonly DepthMap<Type, long, IUpdateSystem> updateComponent = new DepthMap<Type, long, IUpdateSystem>();
        private readonly DepthMap<Type, long, ILoadSystem> loadComponent = new DepthMap<Type, long, ILoadSystem>();
        private readonly ListMap<EventType, IEvent> eventList = new ListMap<EventType, IEvent>();

        private readonly Dictionary<Type, ISystem> systems = new Dictionary<Type, ISystem>();

        private readonly ListMap<Type, Type> attributeTypes = new ListMap<Type, Type>();

        public void RegistSystem(Component component)
        {
            switch (component)
            {
                case IUpdateSystem updateSystem:
                    updateComponent.Add(component.GetType(), component.InstanceId, updateSystem);
                    break;
                case ILoadSystem loadSystem:
                    loadComponent.Add(component.GetType(), component.InstanceId, loadSystem);
                    break;
            }
        }

        public void Remove(Component component)
        {
            updateComponent.Remove(component.GetType(), component.InstanceId);
            loadComponent.Remove(component.GetType(), component.InstanceId);
        }

        public void Update(double delayTime)
        {
            foreach (var kv in updateComponent)
            {
                foreach (var curr in kv.Value)
                {
                    curr.Value.Update(delayTime);
                }
            }
        }

        public void Load()
        {
            foreach (var kv in loadComponent)
            {
                foreach (var curr in kv.Value)
                {
                    curr.Value.Load();
                }
            }
        }

        public List<Type> GetTypes(Type type)
        {
            attributeTypes.TryGetValue(type, out var types);
            return types;
        }

        #region event

        public void Regist(Assembly assembly)
        {
            assemblies[assembly.FullName] = assembly;
            attributeTypes.Clear();

            foreach (var asm in assemblies)
            {
                Type objType = typeof(ObjectAttribute);
                var objectTypes = assembly.GetTypes().ToList().Where(x => x.GetCustomAttribute(objType) != null);
                foreach (Type kv in objectTypes)
                {
                    ObjectAttribute objectAttribute = kv.GetCustomAttribute(objType) as ObjectAttribute;
                    attributeTypes.Add(objectAttribute.GetType(), kv);
                }
            }

            LoadEvent();
            LoadSystem();
        }

        private void LoadEvent()
        {
            eventList.Clear();
            if (attributeTypes.TryGetValue(typeof(EventAttribute), out var types))
            {
                foreach (var type in types)
                {
                    if (!(Activator.CreateInstance(type) is IEvent eve)) continue;

                    EventAttribute attribute = type.GetCustomAttribute(typeof(EventAttribute)) as EventAttribute;
                    eventList.Add(attribute.EventType, eve);
                }
            }
        }

        public void Handle(EventType type)
        {
            if (!eventList.TryGetValue(type, out var eventSystems)) return;
            foreach (var kv in eventSystems)
            {
                if (kv is Event)
                {
                    kv.Run();
                }
            }
        }

        public void Handle<A>(EventType type, A a)
        {
            if (!eventList.TryGetValue(type, out var eventSystems)) return;
            foreach (var kv in eventSystems)
            {
                if (kv is Event<A>)
                {
                    kv.Run(a);
                }
            }
        }

        public void Handle<A, B>(EventType type, A a, B b)
        {
            if (!eventList.TryGetValue(type, out var eventSystems)) return;
            foreach (var kv in eventSystems)
            {
                if (kv is Event<A, B>)
                {
                    kv.Run(a, b);
                }
            }
        }

        public void Handle<A, B, C>(EventType type, A a, B b, C c)
        {
            if (!eventList.TryGetValue(type, out var eventSystems)) return;
            foreach (var kv in eventSystems)
            {
                if (kv is Event<A, B, C>)
                {
                    kv.Run(a, b, c);
                }
            }
        }

        #endregion

        #region System

        public T GetSystem<T>() where T : class, ISystem
        {
            systems.TryGetValue(typeof(T), out var system);
            return system as T;
        }

        private void LoadSystem()
        {
            systems.Clear();
            if (attributeTypes.TryGetValue(typeof(SystemAttribute), out var types))
            {
                foreach (var type in types)
                {
                    if (!(Activator.CreateInstance(type) is ISystem sys)) continue;

                    systems.Add(type, sys);
                }
            }
        }

        #endregion

    }
}
