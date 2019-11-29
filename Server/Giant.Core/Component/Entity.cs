using System.Collections.Generic;

namespace Giant.Core
{
    public class Entity : Component
    {
        private readonly Dictionary<long, Entity> children = new Dictionary<long, Entity>();
        public Dictionary<long, Entity> Children => children;


        private Entity parent;
        public Entity Parent
        {
            get { return parent; }
            set
            {
                if (parent?.InstanceId == value.InstanceId) return;
                parent = value;
            }
        }

        public T GetParent<T>() where T : Entity => parent as T;

        public void AddChild(Entity component)
        {
            children[component.InstanceId] = component;
        }

        public T GetChild<T>(long instanceId) where T : Entity
        {
            children.TryGetValue(instanceId, out var component);
            return component as T;
        }

        public void RemoveChild(long instanceId)
        {
            children.TryGetValue(instanceId, out var entity);
            entity?.Dispose();

            children.Remove(instanceId);
        }

        public override void Dispose()
        {
            children.ForEach(x => x.Value.Dispose());
            children.Clear();

            parent?.RemoveChild(InstanceId);

            base.Dispose();
        }
    }
}
