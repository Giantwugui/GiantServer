using Confluent.Kafka;
using System;

namespace Giant.Utils.Kafka
{
    public class KafkaProducer<K, V> : KafkaClient
    {
        public static ISerializer<K>  DefaultKeySerializer = new ProducerSerialize<K>();
        public static ISerializer<V>  DefaultValueSerializer = new ProducerSerialize<V>();

        private IProducer<K, V> producer;

        public KafkaProducer(string server, string topic, ISerializer<K> ks = null, ISerializer<V> vs = null) : base(topic)
        {
            clientConfig = new ProducerConfig
            {
                BootstrapServers = server,
            };

            producer = GetProducer<K, V>(ks, vs);
        }

        public async void ProduceAsync(V message)
        {
            try
            {
                var dr = await producer.ProduceAsync(this.Topic, new Message<K, V> { Value = message });

                Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
            }
            catch (ProduceException<K, string> e)
            {
                Console.WriteLine($"Delivery failed: {e.Error.Reason}");
            }
        }

        public void Produce(K key, V message)
        {
            try
            {
                producer.Produce(this.Topic, new Message<K, V> { Key = key, Value = message });
                producer.Flush();
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Delivery failed: {e.Error.Reason}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

    }
}