using CryptoTrading.Common.Enums;
using CryptoTrading.Common.MessageBroker;
using CryptoTrading.Common.Records;
using Newtonsoft.Json;
using System.Text;

namespace CryptoTrading.Buying
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var channel = ChannelManager.DeclareQueue("trading");

            while (true)
            {
                decimal value = (decimal)new Random().NextDouble();
                Operation operation = new Operation(OperationType.Buy, value);
                var message = JsonConvert.SerializeObject(operation);
                var body = Encoding.UTF8.GetBytes(message);
                channel.Publish("trading", body);
                Console.WriteLine($"Sent message: {message}");
                await Task.Delay(new Random().Next(10, 100));
            }
        }
    }
}
