using Confluent.Kafka;
using Giant.Share;

namespace Giant.Utils.Kafka
{
    public class KafkaConsumer<K, V> : KafkaClient
    {
        public IDeserializer<K> DefaultKeyDeserializer = new ConsumerDeserialize<K>();
        public IDeserializer<V> DefaultValueDeserializer = new ConsumerDeserialize<V>();

        private IConsumer<K, V> consumer;

        public KafkaConsumer(string server, string topic, IDeserializer<K> kds = null, IDeserializer<V> vds = null) : base(topic)
        {
            clientConfig = new ConsumerConfig
            {
                BootstrapServers = server,
                GroupId = "1",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            this.consumer = GetConsumer<K, V>(kds, vds);
            consumer.Subscribe(this.Topic);
        }

        public ConsumeResult<K, V> Consume()
        {
            var consumeR = consumer.Consume();
            return consumeR;
        }
    }
}