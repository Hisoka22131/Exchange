using Exchange.Common.OperationResult;
using Exchange.Common.Resources;
using Exchange.Core.Mediator.Transactions.Commands.UpdateState;
using MediatR;

namespace Exchange.Web.Endpoints.Admin.Transactions.UpdateState
{
    internal sealed class Endpoint : IEndpoint
    {
        private const string Path = $"/api/v1/{Constants.Groups.Transactions}";

        public Task MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapPut(Path + "/{id:guid}", async (
                    [FromRoute] Guid id,
                    [FromBody] UpdateStateRequest request,
                    IMediator mediator,
                    CancellationToken cancellationToken = default
                ) =>
                {
                    var response = new EmptyOperationResult();

                    try
                    {
                        var command = new UpdateStateCommand(id, request.Status);

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
                .WithName("UpdateTransactionStatus")
                .WithDescription("Обновить статус транзакции")
                .WithTags(Constants.Groups.Transactions)
                .RequireAuthorization("AdminPolicy")
                .WithOpenApi();
            
            return Task.CompletedTask;
        }
    }
}
