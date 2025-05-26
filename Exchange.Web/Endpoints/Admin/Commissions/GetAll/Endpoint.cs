using Exchange.Common.OperationResult;
using Exchange.Common.Resources;
using Exchange.Core.Mediator.Commissions.Query;
using Exchange.Domain.Entities;
using MediatR;

namespace Exchange.Web.Endpoints.Admin.Commissions.GetAll;

internal sealed class Endpoint : IEndpoint
{
    private const string Path = $"/api/v1/{Constants.Groups.Commissions}";
    
    public Task MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet(Path, async (
                IMediator mediator,
                CancellationToken cancellationToken = default
            ) =>
            {
                var response = new OperationResult<IEnumerable<CommissionInfoEntity>>();
                
                try
                {
                    var query = new GetCommissionsQuery();
                    
                    var commissionInfos = await mediator.Send(query, cancellationToken);

                    response.Result = commissionInfos;
                    
                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    
                    response.AddError(CommonErrors.CommonError(ex.Message));

                    return Results.Ok(response);
                }
            })
            .WithName("GetCommissions")
            .WithDescription("Получить список коммиссий")
            .WithTags(Constants.Groups.Commissions)
            .RequireAuthorization("AdminPolicy")
            .WithOpenApi();

        return Task.CompletedTask;
    }
}