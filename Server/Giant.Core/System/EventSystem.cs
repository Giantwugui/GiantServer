using System;

namespace Giant.Core
{
    public class EventSystem
    {
        private static DepthMap<Type, long, IUpdateSystem> updateComponent = new DepthMap<Type, long, IUpdateSystem>();
        private static DepthMap<Type, long, ILoadSystem> loadComponent = new DepthMap<Type, long, ILoadSystem>();

        public void Add(Component component)
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
    }
}
