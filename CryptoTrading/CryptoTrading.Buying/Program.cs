using CryptoTrading.Common.Enums;
using CryptoTrading.Common.Records;
using Newtonsoft.Json;
using System.Text;

namespace CryptoTrading.Buying
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
                        new Operation(OperationType.Buy, value)),
                        Encoding.UTF8,
                        "application/json");

                HttpResponseMessage response = await client.PostAsync("https://localhost:7043/crypto/buy", jsonContent);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine(jsonResponse);

                await Task.Delay(new Random().Next(250, 1500));
            }
        }
    }
}
