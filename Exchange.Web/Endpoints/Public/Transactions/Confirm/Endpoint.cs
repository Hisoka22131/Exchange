using Exchange.Common.OperationResult;
using Exchange.Common.Resources;
using Exchange.Web.Endpoints.Transactions.Confirm;
using MediatR;

namespace Exchange.Web.Endpoints.Public.Transactions.Confirm;

internal sealed class Endpoint : IEndpoint
{
    private const string Path = $"/api/v1/{Constants.Groups.Transactions}/confirm";

    public Task MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost(Path, async (
                ConfirmTransactionsRequest req,
                IMediator mediator,
                CancellationToken cancellationToken = default
            ) =>
            {
                var response = new EmptyOperationResult();
                
                try
                {
                    var command = req.MapToCommand();
                    
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
            .WithName("TransactionsConfirmEndpoint")
            .WithDescription("Подтвердить транзакцию")
            .WithTags(Constants.Groups.Transactions)
            .WithOpenApi();

        return Task.CompletedTask;
    }
}