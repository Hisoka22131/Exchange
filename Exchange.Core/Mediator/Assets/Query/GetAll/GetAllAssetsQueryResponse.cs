using Exchange.Common.Models;

namespace Exchange.Core.Mediator.Assets.Query.GetAll;

public record GetAllAssetsQueryResponse(
    IList<DigitalAsset> FiatCurrencies,
    IList<DigitalAsset> CryptoCurrencies
);