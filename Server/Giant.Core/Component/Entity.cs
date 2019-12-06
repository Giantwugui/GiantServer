using System.Collections.Generic;

namespace Giant.Core
{
    public class Entity : Component
    {
        private readonly Dictionary<long, Entity> children = new Dictionary<long, Entity>();
        public Dictionary<long, Entity> Children => children;


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
            if (children.TryGetValue(instanceId, out var entity))
            {
                if (!entity.IsDisposed())
                {
                    entity.Dispose();
                }
            }

            children.Remove(instanceId);
        }

        public override void Dispose()
        {
            if (IsDisposed()) return;

            long instanceId = InstanceId;
            base.Dispose();

            children.ForEach(x => x.Value.Dispose());
            children.Clear();

            GetParent<Entity>()?.RemoveChild(instanceId);
        }
    }
}
