using System;
using Confluent.Kafka;
using Giant.Share;

namespace Giant.Utils.Kafka
{
    public abstract class KafkaClient
    {
        protected ClientConfig clientConfig;
        public string Topic { get; private set; }

        public KafkaClient(string topic)
        {
            this.Topic = topic;
        }

        protected IProducer<K, V> GetProducer<K, V>(ISerializer<K> KSerializer = null, ISerializer<V> VSerializer = null)
        {
            var builder = new ProducerBuilder<K, V>(clientConfig);
            if (KSerializer != null)
            {
                builder.SetKeySerializer(KSerializer);
            }
            if (VSerializer != null)
            {
                builder.SetValueSerializer(VSerializer);
            }
            return builder.Build();
        }

        protected IConsumer<K, V> GetConsumer<K, V>(IDeserializer<K> KDeserialize = null, IDeserializer<V> VDeserializer = null)
        {
            var builder = new ConsumerBuilder<K, V>(clientConfig);
            if (KDeserialize != null)
            {
                builder.SetKeyDeserializer(KDeserialize);
            }
            if (VDeserializer != null)
            {
                builder.SetValueDeserializer(VDeserializer);
            }
            return builder.Build();
        }
    }

    public class ConsumerDeserialize<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context) => data.ToArray().FromJsonBytes<T>();
    }

    public class ProducerSerialize<T> : ISerializer<T>
    {
        public byte[] Serialize(T data, SerializationContext context) => data.ToJsonBytes();
    }
}
