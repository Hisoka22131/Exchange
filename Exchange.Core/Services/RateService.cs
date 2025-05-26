using Exchange.Common.Enums;
using Exchange.Core.Options;
using Exchange.Domain.Entities;
using Exchange.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace Exchange.Core.Services;

internal sealed class RateService : IRateService
{
    private readonly ICacheService _cacheService;
    private readonly IExchangeMetaData _exchangeMetaData;
    private readonly PercentOptions _percentOptions;

    public RateService(ICacheService cacheService, IExchangeMetaData exchangeMetaData, IOptions<PercentOptions> options)
    {
        _cacheService = cacheService;
        _exchangeMetaData = exchangeMetaData;
        _percentOptions = options.Value;
    }

    public Task<decimal> CalculateRateAsync(
        Currency source,
        Currency target,
        CancellationToken ct = default
    )
    {
        if (source == target)
            throw new ArgumentException("Валюты отправления и получения должны быть разные");

        var from = source.GetCurrency();
        var to = target.GetCurrency();

        var isFiat = from.IsFiat();

        // меняю местами, потому что, например, курс USD/USDT не существует
        if (isFiat)
        {
            (from, to) = (to, from);
        }

        var quote = _cacheService.Get<CurrencyInfo>("allCurrencies")?.Data?[$"{from}"].Quote[$"{to}"].Price;

        if (quote is null)
            throw new ArgumentNullException(nameof(quote));

        if (_exchangeMetaData.GetCurrencyFrom() is Currency.RUB)
        {
            quote *= 1 + _percentOptions.Rub.Sell / 100m;
        }
        
        if (_exchangeMetaData.GetCurrencyTo() is Currency.RUB)
        {
            quote *= 1 - _percentOptions.Rub.Buy / 100m;
        }

        var rate = isFiat ? (1 / quote.Value) : quote.Value;

        return Task.FromResult(rate);
    }
}