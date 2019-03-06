using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Giant.Model
{
    public abstract class Entity : ComponentWithId
    {
        public Entity() : base()
        {
        }

        public Entity(long id) : base(id)
        {
        }

        public void AddComponent(Component component)
        {
            components.Add(component);

            Type type = component.GetType();

            componentDict[type] = component;
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            base.Dispose();

            foreach (Component component in components)
            {
                try
                {
                    component.Dispose();
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }

            components.Clear();
            componentDict.Clear();
        }

        /// <summary>
        /// 所有下挂组件
        /// </summary>
        [BsonElement("Components")]
        [BsonIgnoreIfNull]
        private HashSet<Component> components = new HashSet<Component>();

        /// <summary>
        /// 所有类型组件
        /// </summary>
        [BsonIgnore]
        private Dictionary<Type, Component> componentDict = new Dictionary<Type, Component>();
    }
}
