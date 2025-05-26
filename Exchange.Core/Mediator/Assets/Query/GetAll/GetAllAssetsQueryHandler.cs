using Exchange.Common.Constants;
using Exchange.Common.Enums;
using MediatR;

namespace Exchange.Core.Mediator.Assets.Query.GetAll;

internal sealed class GetAllAssetsQueryHandler : IRequestHandler<GetAllAssetsQuery, GetAllAssetsQueryResponse>
{
    public Task<GetAllAssetsQueryResponse> Handle(GetAllAssetsQuery request, CancellationToken cancellationToken)
    {
        var fiatCurrencies = DigitalAssets.Currencies
            .Where(c => c.Value.IsFiat())
            .ToList();

        var cryptoCurrencies = DigitalAssets.Currencies
            .Where(c => c.Value.IsCrypto())
            .ToList();
        
        var response = new GetAllAssetsQueryResponse(fiatCurrencies, cryptoCurrencies);

        return Task.FromResult(response);
    }
}