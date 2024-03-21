using CryptoTrading.Common.Enums;
using CryptoTrading.Common.MessageBroker;
using CryptoTrading.Common.Records;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System.Text;

namespace CryptoTrading.QueueManager
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await Task.Delay(45000);

            var channel = ChannelManager.DeclareQueue("trading");
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (a, b) =>
            {
                var body = b.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var operation = JsonConvert.DeserializeObject<Operation>(message);
                Console.WriteLine($"Received message: {message}");

                switch (operation.OperationType)
                {
                    case OperationType.Buy:
                        await RegisterPurchase(operation);
                        break;

                    case OperationType.Sell:
                        await RegisterSale(operation);
                        break;
                }
            };

            channel.Consume(consumer);

            Console.ReadKey();
        }

        private static async Task RegisterPurchase(Operation operation)
        {
            using var client = new HttpClient();
            using StringContent jsonContent =
                new(JsonConvert.SerializeObject(
                    new Operation(OperationType.Buy, operation.Value)),
                    Encoding.UTF8,
                    "application/json");

            HttpResponseMessage response = await client.PostAsync("https://localhost:7043/crypto/buy", jsonContent);
            var jsonResponse = await response.Content.ReadAsStringAsync();
        }

        private static async Task RegisterSale(Operation operation)
        {
            using var client = new HttpClient();
            using StringContent jsonContent =
                new(JsonConvert.SerializeObject(
                    new Operation(OperationType.Sell, operation.Value)),
                    Encoding.UTF8,
                    "application/json");

            HttpResponseMessage response = await client.PostAsync("https://localhost:7043/crypto/sell", jsonContent);
            var jsonResponse = await response.Content.ReadAsStringAsync();
        }
    }
}
