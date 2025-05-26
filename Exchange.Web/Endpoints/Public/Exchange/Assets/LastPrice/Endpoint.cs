using Exchange.Common.OperationResult;
using Exchange.Common.Resources;
using Exchange.Core.Mediator.Assets.Query;
using Exchange.Domain.Entities;
using MediatR;

namespace Exchange.Web.Endpoints.Public.Exchange.Assets.LastPrice;

internal sealed class Endpoint : IEndpoint
{
    private const string Path = $"/api/v1/{Constants.Groups.Assets}/price";
    
    public Task MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet(Path, async (
                IMediator mediator,
                CancellationToken cancellationToken = default
            ) =>
            {
                var response = new OperationResult<PriceInfo[]>();
                
                try
                {
                    response.Result = await mediator.Send(new GetAssetsPriceQuery(), cancellationToken);
                    
                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    
                    response.AddError(CommonErrors.CommonError(ex.Message));

                    return Results.Ok(response);
                }
            })
            .WithName("GetAssetsPrice")
            .WithDescription("Получить изменения курса за последний час")
            .WithTags(Constants.Groups.Assets)
            .WithOpenApi();

        return Task.CompletedTask;
    }
}