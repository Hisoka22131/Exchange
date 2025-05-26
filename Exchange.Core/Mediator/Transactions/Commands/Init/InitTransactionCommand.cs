using Exchange.Common.Enums;
using Exchange.Domain.Enums;
using MediatR;

namespace Exchange.Core.Mediator.Transactions.Commands.Init;

public record InitTransactionCommand(
    Currency CurrencyFrom,
    Currency CurrencyTo,
    decimal AmountFrom,
    decimal AmountTo,
    decimal FeeInUsdt,
    NetworkCode NetworkCryptoCode,
    NetworkCode NetworkFiatCode,
    string? WalletAddressUser,
    string City,
    string PhoneNumber,
    long UserId,
    string UserName
) : IRequest<InitTransactionCommandResponse>;