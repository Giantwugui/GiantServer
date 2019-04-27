using MongoDB.Bson.Serialization.Attributes;
using System;
using Giant.Model.Helper;

namespace Giant.Model
{
    [BsonIgnoreExtraElements]
    public abstract class Component : Object, IDisposable
    {
        [BsonIgnore]
        [BsonDefaultValue(0L)]
        public long InstanceId { get; protected set; }

        private bool isFromPool;

        [BsonIgnore]
        public bool IsFromPool
        {
            get { return isFromPool; }
            set
            {
                isFromPool = value;

                if (!isFromPool && InstanceId == 0)
                {
                    InstanceId = IdGenerater.GenerateId();
                }
            }
        }

        [BsonIgnore]
        public bool IsDisposed
        {
            get { return InstanceId == 0; }
        }

        [BsonIgnore]
        private Component parent;

        [BsonIgnore]
        public Component Parent
        {
            get { return parent; }
            set { parent = value; }
        }


        protected Component()
        {
            InstanceId = IdGenerater.GenerateId();
        }

        public T GetParent<T>() where T: Component
        {
            return parent as T;
        }

        public virtual void Dispose()
        {
            InstanceId = 0;

            if (IsFromPool)
            {
                Game.ObjectPool.Recycle(this);
            }
        }

    }
}
