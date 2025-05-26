using Exchange.Domain.Entities;

namespace Exchange.CoinMarketCap.Extensions;

internal static class CurrencyInfoExtensions
{
    public static void AddRubPmrCurrency(this CurrencyInfo currencyInfo, string customCurrencySymbol, decimal usdToCustomRate)
    {
        if (currencyInfo?.Data == null || string.IsNullOrWhiteSpace(customCurrencySymbol) || usdToCustomRate <= 0)
            throw new ArgumentException("Invalid input data.");

        foreach (var currencyData in currencyInfo.Data.Values)
        {
            if (currencyData.Quote.ContainsKey(customCurrencySymbol))
                continue;

            if (currencyData.Quote.TryGetValue("USD", out var usdQuote))
            {
                var customPrice = usdQuote.Price * usdToCustomRate;

                currencyData.Quote[customCurrencySymbol] = new Quote
                {
                    Price = customPrice
                };
            }
        }
    }
}