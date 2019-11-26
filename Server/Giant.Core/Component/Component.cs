using System;
using System.Collections.Generic;

namespace Giant.Core
{
    public partial class Component : IDisposable
    {
        private Dictionary<Type, Component> componentes = new Dictionary<Type, Component>();
        private Dictionary<long, Component> children = new Dictionary<long, Component>();

        public long InstanceId { get; set; }
        public Dictionary<long, Component> Children => children;

        public Component()
        {
            InstanceId = IdGenerator.NewId;
        }

        public void AddChild(Component component)
        {
            children[component.InstanceId] = component;
        }

        public Component GetChild(long instanceId)
        {
            children.TryGetValue(instanceId, out var component);
            return component;
        }

        public virtual void Dispose()
        {
            Scene.EventSystem.Remove(this);
        }
    }
}
