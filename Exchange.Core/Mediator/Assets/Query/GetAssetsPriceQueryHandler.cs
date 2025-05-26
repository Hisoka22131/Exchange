using Exchange.Common.Enums;
using Exchange.Core.Extensions;
using Exchange.Domain.Entities;
using Exchange.Domain.Enums;
using Exchange.Domain.Interfaces;
using MediatR;

namespace Exchange.Core.Mediator.Assets.Query;

internal sealed class GetAssetsPriceQueryHandler : IRequestHandler<GetAssetsPriceQuery, PriceInfo[]>
{
    private readonly ICacheService _cacheService;

    public GetAssetsPriceQueryHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public Task<PriceInfo[]> Handle(GetAssetsPriceQuery request, CancellationToken cancellationToken)
    {
        var currencyInfo = _cacheService.Get<CurrencyInfo>("allCurrencies");

        if (currencyInfo == null)
            throw new ArgumentNullException(nameof(currencyInfo));

        var priceInfos = currencyInfo.Data
            .SelectMany(currencyData =>
                currencyData.Value.Quote.Where(x => x.Key is nameof(Currency.USD))
                    .Select(quote =>
                        new PriceInfo
                        {
                            Name = currencyData.Value.Name ?? string.Empty,
                            Price = quote.Value.Price.RoundToDecimalSevenPlaces(),
                            IsGrow = quote.Value.IsGrow
                        }))
            .ToArray();

        return Task.FromResult(priceInfos);
    }
}