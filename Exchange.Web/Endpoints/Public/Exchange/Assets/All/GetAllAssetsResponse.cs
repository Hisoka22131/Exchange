using Exchange.Common.Models;

namespace Exchange.Web.Endpoints.Public.Exchange.Assets.All;

public record GetAllAssetsResponse(
    IList<DigitalAsset> FiatCurrencies,
    IList<DigitalAsset> CryptoCurrencies
);