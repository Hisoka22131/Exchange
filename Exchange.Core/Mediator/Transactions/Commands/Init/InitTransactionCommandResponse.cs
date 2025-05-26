namespace Exchange.Core.Mediator.Transactions.Commands.Init;

public record InitTransactionCommandResponse(
    bool FromCrypto,
    Guid TransactionId,
    decimal AmountFrom,
    decimal AmountTo,
    decimal FeeInUsdt,
    decimal FeeInCurrency,
    string? Address = null,
    string? PhoneNumber = null
);