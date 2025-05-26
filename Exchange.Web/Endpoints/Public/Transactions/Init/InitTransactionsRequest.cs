using Exchange.Common.Enums;
using Exchange.Core.Mediator.Transactions.Commands.Init;

namespace Exchange.Web.Endpoints.Public.Transactions.Init;

public record InitTransactionsRequest(
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
)
{
    public InitTransactionCommand MapToCommand()
    {
        return new InitTransactionCommand(
            CurrencyFrom: CurrencyFrom,
            CurrencyTo: CurrencyTo,
            AmountFrom: AmountFrom,
            AmountTo: AmountTo,
            FeeInUsdt: FeeInUsdt,
            NetworkCryptoCode: NetworkCryptoCode,
            NetworkFiatCode: NetworkFiatCode,
            WalletAddressUser: WalletAddressUser,
            City: City,
            PhoneNumber: PhoneNumber,
            UserId: UserId,
            UserName: UserName);
    }
};