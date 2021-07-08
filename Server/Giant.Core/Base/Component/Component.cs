using System;
using System.Collections.Generic;

namespace Giant.Core
{
    public partial class Component : IDisposable
    {
        private Dictionary<Type, Component> componentes = new Dictionary<Type, Component>();

        public long InstanceId { get; set; }

        private Component parent;
        public Component Parent
        {
            get { return parent; }
            set
            {
                if (parent?.InstanceId == value.InstanceId) return;
                parent = value;
            }
        }

        public T GetParent<T>() where T : Component => parent as T;

        public Component()
        {
            InstanceId = IdGenerator.NewId;
        }

        public virtual void Dispose()
        {
            foreach (var kv in componentes)
            {
                kv.Value.Dispose();
            }

            componentes.Clear();

            Scene.EventSystem.Remove(this);

            InstanceId = 0;
        }

        public bool IsDisposed=> InstanceId == 0;
    }
}
