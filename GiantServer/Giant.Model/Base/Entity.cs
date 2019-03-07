using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Giant.Model
{
    public abstract class Entity : ComponentWithId
    {
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


        public Entity() : base()
        {
        }

        public Entity(long id) : base(id)
        {
        }

        public void AddComponent<T>() where T: Component, new()
        {
            Type type = typeof(T);

            if (componentDict.ContainsKey(type))
            {
                throw new Exception($"compoent {type.Name} all ready exists in component {this.Id} {this.GetType()}!");
            }

            T component = ComponentFactory.CreateWithParent<T>(this, this.IsFromPool);

            components.Add(component);

            componentDict[type] = component;
        }

        public void AddComponent<T, P1>(P1 p1) where T : Component, new()
        {
            Type type = typeof(T);

            if (componentDict.ContainsKey(type))
            {
                throw new Exception($"compoent {type.Name} all ready exists in component {this.Id} {this.GetType()}!");
            }

            T component = ComponentFactory.CreateWithParent<T, P1>(this, p1, this.IsFromPool);

            components.Add(component);

            componentDict[type] = component;
        }

        public void AddComponent<T, P1, P2>(P1 p1, P2 p2) where T : Component, new()
        {
            Type type = typeof(T);

            if (componentDict.ContainsKey(type))
            {
                throw new Exception($"compoent {type.Name} all ready exists in component {this.Id} {this.GetType()}!");
            }

            T component = ComponentFactory.CreateWithParent<T, P1, P2>(this, p1, p2, this.IsFromPool);

            components.Add(component);

            componentDict[type] = component;
        }

        public void AddComponent<T, P1, P2, P3>(P1 p1, P2 p2, P3 p3) where T : Component, new()
        {
            Type type = typeof(T);

            if (componentDict.ContainsKey(type))
            {
                throw new Exception($"compoent {type.Name} all ready exists in component {this.Id} {this.GetType()}!");
            }

            T component = ComponentFactory.CreateWithParent<T, P1, P2, P3>(this, p1, p2, p3, this.IsFromPool);

            components.Add(component);

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
    }
}
