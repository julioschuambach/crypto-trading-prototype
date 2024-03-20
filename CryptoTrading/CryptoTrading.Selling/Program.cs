using CryptoTrading.Common.Enums;
using CryptoTrading.Common.Records;
using Newtonsoft.Json;
using System.Text;

namespace CryptoTrading.Selling
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                decimal value = (decimal)new Random().NextDouble();

                using var client = new HttpClient();
                using StringContent jsonContent =
                    new(JsonConvert.SerializeObject(
                        new Operation(OperationType.Sell, value)),
                        Encoding.UTF8,
                        "application/json");

                HttpResponseMessage response = await client.PostAsync("https://localhost:7043/crypto/sell", jsonContent);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine(jsonResponse);

                await Task.Delay(new Random().Next(250, 1500));
            }
        }
    }
}
