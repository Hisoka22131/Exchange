using Exchange.Common.OperationResult;
using Exchange.Common.Resources;
using Exchange.Core.Mediator.Assets.Query.GetAll;
using MediatR;

namespace Exchange.Web.Endpoints.Public.Exchange.Assets.All;

internal sealed class Endpoint : IEndpoint
{
    private const string Path = $"/api/v1/{Constants.Groups.Assets}/all";
    
    public Task MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet(Path, async (
                IMediator mediator,
                CancellationToken cancellationToken = default
            ) =>
            {
                var response = new OperationResult<GetAllAssetsResponse>();
                
                try
                {
                    var mediatorResponse = await mediator.Send(new GetAllAssetsQuery(), cancellationToken);
                    
                    response.Result = new GetAllAssetsResponse(mediatorResponse.FiatCurrencies, mediatorResponse.CryptoCurrencies);
                    
                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    
                    response.AddError(CommonErrors.CommonError(ex.Message));

                    return Results.Ok(response);
                }
            })
            .WithName("GetAllAssets")
            .WithDescription("Получить все активы")
            .WithTags(Constants.Groups.Assets)
            .WithOpenApi();

        return Task.CompletedTask;
    }
}