using Exchange.Common.OperationResult;
using Exchange.Common.Resources;
using Exchange.Core.Mediator.Commissions.Command;
using MediatR;

namespace Exchange.Web.Endpoints.Admin.Commissions.Update;

internal sealed class Endpoint : IEndpoint
{
    private const string Path = $"/api/v1/{Constants.Groups.Commissions}";

    public Task MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPut(Path, async (
                [FromBody] IEnumerable<CommissionUpdateRequest> commissionInfos,
                IMediator mediator,
                CancellationToken cancellationToken = default
            ) =>
            {
                var response = new EmptyOperationResult();

                try
                {
                    var command = new UpdateCommissionsCommand(
                        commissionInfos.Select(x => x.MapToEntity())
                    );

                    await mediator.Send(command, cancellationToken);

                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);

                    response.AddError(CommonErrors.CommonError(ex.Message));

                    return Results.Ok(response);
                }
            })
            .WithName("UpdateCommissions")
            .WithDescription("Обновить список коммиссий")
            .WithTags(Constants.Groups.Commissions)
            .RequireAuthorization("AdminPolicy")
            .WithOpenApi();

        return Task.CompletedTask;
    }
}