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
            Scene.EventSystem.Remove(this);
        }
    }
}
