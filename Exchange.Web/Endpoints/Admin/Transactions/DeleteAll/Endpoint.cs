using Exchange.Common.OperationResult;
using Exchange.Common.Resources;
using Exchange.Core.Mediator.Transactions.Commands.Delete;
using Exchange.Core.Mediator.Transactions.Query.GetById;
using Exchange.Web.Dto;
using MediatR;

namespace Exchange.Web.Endpoints.Admin.Transactions.DeleteAll;

internal sealed class Endpoint : IEndpoint
{
    private const string Path = $"/api/v1/{Constants.Groups.Transactions}";
    
    public Task MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapDelete(Path, async (
                IMediator mediator,
                CancellationToken cancellationToken = default
            ) =>
            {
                var response = new EmptyOperationResult();

                try
                {
                    var command = new DeleteTransactionCommand();
                    
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
            .WithName("DeleteAllTransactions")
            .WithDescription("Удалить транзакции")
            .WithTags(Constants.Groups.Transactions)
            .RequireAuthorization("AdminPolicy")
            .WithOpenApi();

        return Task.CompletedTask;
    }
}