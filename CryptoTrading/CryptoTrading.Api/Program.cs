using Microsoft.AspNetCore.Mvc;
using CryptoTrading.Common.Records;

namespace CryptoTrading.Api
{
    public class Program
    {
        private static decimal _balance = 150M;

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.MapPost("/crypto/buy", async ([FromBody] Operation operation) =>
            {
                var balance = _balance;
                _balance -= operation.Value;
                Console.WriteLine($"An user has bought {balance - _balance} crypto at {DateTime.Now}.");

                return await Task.FromResult(Results.Ok(new 
                {
                    Operation = operation.OperationType.ToString(),
                    Message = $"You bought {operation.Value} successfully.",
                    Balance = $"The balance is now {_balance}"
                }));
            });

            app.MapPost("/crypto/sell", async ([FromBody] Operation operation) =>
            {
                var balance = _balance;
                _balance += operation.Value;
                Console.WriteLine($"An user has sold {_balance - balance} crypto at {DateTime.Now}.");

                return await Task.FromResult(Results.Ok(new
                {
                    Operation = operation.OperationType.ToString(),
                    Message = $"You sold {operation.Value} successfully.",
                    Balance = $"The balance is now {_balance}"
                }));
            });

            app.Run();
        }
    }
}
