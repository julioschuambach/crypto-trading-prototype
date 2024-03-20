using CryptoTrading.Common.Enums;

namespace CryptoTrading.Common.Records
{
    public record Operation(OperationType OperationType, decimal Value);
}
