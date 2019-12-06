using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using Giant.Share;
using Giant.Utils.Kafka;

namespace Giant.Test
{
    internal class Program
    {
        private static KafkaProducer<int, Player> producer;
        private static KafkaConsumer<int, Player> consumer;
        private static int messageIndex = 0;

        private static void Main(string[] args)
        {
            try
            {
                string host = "192.168.1.176:9092";
                string topic = "player";
                producer = new KafkaProducer<int, Player>(host, topic);
                consumer = new KafkaConsumer<int, Player>(host, topic);

                ConsumMessage();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            while (true)
            {
                Thread.Sleep(1);

                string message = Console.ReadLine();
                Player player = new Player() { Account = message, Name = message.ToString() };

                producer.Produce(++messageIndex, player);
            }
        }

        private static void ConsumMessage()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var result = consumer.Consume();
                    Player player = result.Message.Value;

                    Console.WriteLine($"consume mesage {result.Message.Key} message {result.Message.Value.ToJson()}");
                }
            });
        }
    }
}