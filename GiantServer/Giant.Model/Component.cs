using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Giant.Model
{
    [BsonIgnoreExtraElements]
    public abstract class Component : Object, IDisposable
    {
        public Component()
        {
            InstanceId = IdGenerater.GenerateId();
        }

        public bool IsDisposed()
        {
            return InstanceId == 0;
        }

        public virtual void Dispose()
        {
            InstanceId = 0;
        }

        [BsonIgnore]
        [BsonDefaultValue(0L)]
        public long InstanceId { get; protected set; }

        [BsonIgnore]
        private bool isFromPool;
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

    }
}
