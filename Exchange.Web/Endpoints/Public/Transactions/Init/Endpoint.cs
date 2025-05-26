using Exchange.Common.OperationResult;
using Exchange.Common.Resources;
using MediatR;

namespace Exchange.Web.Endpoints.Public.Transactions.Init;

internal sealed class Endpoint : IEndpoint
{
    private const string Path = $"/api/v1/{Constants.Groups.Transactions}/init";

    public Task MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost(Path, async (
                InitTransactionsRequest req,
                IMediator mediator,
                CancellationToken cancellationToken = default
            ) =>
            {
                var response = new OperationResult<InitTransactionsResponse>();
                
                try
                {
                    var command = req.MapToCommand();
                    
                    var commandResponse = await mediator.Send(command, cancellationToken);
                    
                    response.Result = InitTransactionsResponse.MapToResponse(commandResponse);
                    
                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    
                    response.AddError(CommonErrors.CommonError(ex.Message));

                    return Results.Ok(response);
                }
            })
            .WithName("TransactionsInitEndpoint")
            .WithDescription("Инициировать транзакцию")
            .WithTags(Constants.Groups.Transactions)
            .WithOpenApi();

        return Task.CompletedTask;
    }
}