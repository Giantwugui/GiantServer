using System.Collections.Generic;

namespace Giant.Core
{
    public class Entity : Component
    {
        private Dictionary<long, Entity> children = new Dictionary<long, Entity>();
        public Dictionary<long, Entity> Children => children;

        public void AddChild(Entity component)
        {
            children[component.InstanceId] = component;
        }

        public Entity GetChild(long instanceId)
        {
            children.TryGetValue(instanceId, out var component);
            return component;
        }

        public void RemoveChild(long instanceId)
        {
            children.Remove(instanceId);
        }

        public override void Dispose()
        {
            children.ForEach(x => x.Value.Dispose());
            children.Clear();

            base.Dispose();
        }
    }
}
