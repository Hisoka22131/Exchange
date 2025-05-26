// ReSharper disable InconsistentNaming

namespace Exchange.Common.Enums;

public enum Currency : byte
{
    UNKNOWN = 0,

    USDT = 1,
    BTC = 2,
    ETH = 3,
    TRX = 4,
    LTC = 5,

    EUR = 6,
    USD = 7,
    RUP = 8,
    MDL = 9,
    RUB = 10,
}

public static class CurrencyExtensions
{
    public static Currency GetCurrency(this Currency currency)
    {
        return currency switch
        {
            Currency.UNKNOWN => Currency.UNKNOWN,
            Currency.USDT => Currency.USDT,
            Currency.BTC => Currency.BTC,
            Currency.ETH => Currency.ETH,
            Currency.TRX => Currency.TRX,
            Currency.LTC => Currency.LTC,
            Currency.EUR => Currency.EUR,
            Currency.USD => Currency.USD,
            Currency.RUP => Currency.RUP,
            Currency.MDL => Currency.MDL,
            Currency.RUB => Currency.RUB,
        };
    }

    public static bool IsCrypto(this Currency currency)
    {
        return currency switch
        {
            Currency.USDT or Currency.BTC or Currency.ETH or Currency.TRX or Currency.LTC => true,
            _ => false
        };
    }

    public static bool IsFiat(this Currency currency)
    {
        return currency switch
        {
            Currency.EUR or Currency.USD or Currency.RUP or Currency.MDL or Currency.RUB => true,
            _ => false
        };
    }

    public static bool CanExchange(this Currency fromCurrency, Currency toCurrency)
    {
        if (fromCurrency.IsCrypto() && toCurrency.IsCrypto() || fromCurrency.IsFiat() && toCurrency.IsFiat())
            return false;

        return true;
    }
}