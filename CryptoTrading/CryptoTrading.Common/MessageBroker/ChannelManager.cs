using RabbitMQ.Client;

namespace CryptoTrading.Common.MessageBroker
{
    public static class ChannelManager
    {
        public static IModel DeclareQueue(string queue, bool durable = false, bool exclusive = false, bool autoDelete = false, IDictionary<string, object>? arguments = null)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: queue,
                                 durable: durable,
                                 exclusive: exclusive,
                                 autoDelete: autoDelete,
                                 arguments: arguments);

            return channel;
        }

        public static void Publish(this IModel model, string queue, ReadOnlyMemory<byte> body, string exchange = "", IBasicProperties? basicProperties = null)
        {
            model.BasicPublish(exchange: exchange,
                               routingKey: queue,
                               basicProperties: basicProperties,
                               body: body);
        }

        public static void Consume(this IModel model, IBasicConsumer consumer, string queue = "", bool autoAck = true)
        {
            model.BasicConsume(queue: string.IsNullOrEmpty(queue) ? model.CurrentQueue : queue,
                               autoAck: autoAck,
                               consumer: consumer);
        }
    }
}
