using Exchange.Common.Enums;

namespace Exchange.Core.Records;

public record TransactionCacheInfo(
    Guid TransactionId,
    Currency CurrencyFrom,
    Currency CurrencyTo,
    decimal AmountFrom,
    decimal AmountTo,
    decimal CommissionInUsdt,
    string City,
    string PhoneNumber,
    NetworkCode CryptoNetworkCode,
    string CryptoNetworkName,
    NetworkCode FiatNetworkCode,
    string FiatNetworkName,
    long TelegramUserId,
    string TelegramUserName, 
    string WalletAddressAdmin,
    string WalletAddressUser,
    decimal AmountToInUsdt
);