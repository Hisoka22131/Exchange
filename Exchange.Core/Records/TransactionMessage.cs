using System.Text;
using System.Text.RegularExpressions;
using Exchange.Common.Enums;
using Exchange.Domain.Entities;

namespace Exchange.Core.Records;

internal sealed record TransactionMessage
{
    private readonly TransactionEntity _transaction;
    
    public TransactionMessage(TransactionEntity transaction)
    {
        _transaction = transaction;
    }

    public override string ToString()
    {
        var messageBuilder = new StringBuilder();

        messageBuilder.AppendLine("*Была сделана транзакция!*");
        messageBuilder.AppendLine();
        
        messageBuilder.AppendLine($"*Идентификатор*: `{_transaction.Id}`");

        messageBuilder.AppendLine($"*Счет поступления:* `{_transaction.WalletAddressAdmin}`");

        if (_transaction.CurrencyFrom.IsCrypto())
            messageBuilder.AppendLine($"*Сеть:* `{_transaction.CryptoNetworkName}`");
        
        messageBuilder.AppendLine($"*Сумма:* {_transaction.AmountFrom.ToString("G29")} {_transaction.CurrencyFrom}");

        messageBuilder.AppendLine();

        messageBuilder.AppendLine($"*Счет отправки:* `{_transaction.WalletAddressUser}`");
        
        if (_transaction.CurrencyFrom.IsFiat())
            messageBuilder.AppendLine($"*Сеть:* `{_transaction.CryptoNetworkName}`");
        
        messageBuilder.AppendLine($"*Сумма:* {_transaction.AmountTo.ToString("G29")} {_transaction.CurrencyTo}");
        messageBuilder.AppendLine($"*Комиссия:* {_transaction.Commission.ToString("G29")} {Currency.USDT}");
        messageBuilder.AppendLine($"*Город:* {_transaction.City}");
        messageBuilder.AppendLine($"*Номер телефона:* {_transaction.PhoneNumberUser}");
        messageBuilder.AppendLine($"*Имя пользователя:* @{EscapeMarkdownV2(_transaction.User.TelegramUserName)}");

        return messageBuilder.ToString();
    }

    private static string EscapeMarkdownV2(string text)
    {
        // Экранируем все символы, которые не предшествуют обратной косой чертой
        return Regex.Replace(text, @"(?<!\\)([_*\[\]()~`>#+\-=|{}\.!])", @"\$1");
    }
}