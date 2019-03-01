using MongoDB.Bson.Serialization.Attributes;

namespace Giant.Model
{
    [BsonIgnoreExtraElements]
    public abstract class ComponentWithId : Component
    {
        protected ComponentWithId()
        {
            Id = InstanceId;
        }

        protected ComponentWithId(long id)
        {
            Id = id;
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        [BsonId]
        [BsonElement]
        [BsonIgnoreIfDefault]
        [BsonDefaultValue(0L)]
        public long Id { get; protected set; }
    }
}
