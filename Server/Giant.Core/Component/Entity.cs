using System.Collections.Generic;

namespace Giant.Core
{
    public class Entity : Component
    {
        private Dictionary<long, Component> children = new Dictionary<long, Component>();
        public Dictionary<long, Component> Children => children;

        public void AddChild(Component component)
        {
            children[component.InstanceId] = component;
        }

        public Component GetChild(long instanceId)
        {
            children.TryGetValue(instanceId, out var component);
            return component;
        }
    }
}
