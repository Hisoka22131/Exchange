using Exchange.Core.Mediator.Transactions.Commands.Init;

namespace Exchange.Web.Endpoints.Public.Transactions.Init;

public record InitTransactionsResponse(
    bool FromCrypto,
    Guid TransactionId,
    string? Address,
    decimal AmountFrom,
    decimal AmountTo,
    decimal FeeInUsdt,
    decimal FeeInCurrency,
    string? PhoneNumberAdmin
)
{
    public static InitTransactionsResponse MapToResponse(InitTransactionCommandResponse commandResponse)
    {
        return new InitTransactionsResponse(
            FromCrypto: commandResponse.FromCrypto,
            TransactionId: commandResponse.TransactionId,
            Address: commandResponse.Address,
            PhoneNumberAdmin : commandResponse.PhoneNumber,
            AmountFrom: commandResponse.AmountFrom,
            AmountTo: commandResponse.AmountTo,
            FeeInUsdt: commandResponse.FeeInUsdt,
            FeeInCurrency: commandResponse.FeeInCurrency);
    }
}