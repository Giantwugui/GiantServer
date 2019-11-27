using System;
using System.Collections.Generic;

namespace Giant.Core
{
    public partial class Component : IDisposable
    {
        private Dictionary<Type, Component> componentes = new Dictionary<Type, Component>();

        public long InstanceId { get; set; }

        public Component()
        {
            InstanceId = IdGenerator.NewId;
        }

        public virtual void Dispose()
        {
            foreach (var kv in componentes)
            {
                Scene.Pool.RemoveChild(kv.Value.InstanceId);
                kv.Value.Dispose();
            }

            componentes.Clear();

            Scene.EventSystem.Remove(this);
            InstanceId = 0;
        }
    }
}
