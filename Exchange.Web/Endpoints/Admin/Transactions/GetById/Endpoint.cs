using Exchange.Common.OperationResult;
using Exchange.Common.Resources;
using Exchange.Core.Mediator.Transactions.Query.GetById;
using Exchange.Web.Dto;
using MediatR;

namespace Exchange.Web.Endpoints.Admin.Transactions.GetById;

internal sealed class Endpoint : IEndpoint
{
    private const string Path = $"/api/v1/{Constants.Groups.Transactions}";

    public Task MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet(Path + "/{id:guid}", async (
                [FromRoute] Guid id,
                IMediator mediator,
                CancellationToken cancellationToken = default
            ) =>
            {
                var response = new OperationResult<TransactionDto>();

                try
                {
                    
                    var query = new GetTransactionQuery(id);
                    
                    var transaction = await mediator.Send(query, cancellationToken);

                    if (transaction is null)
                    {
                        throw new ArgumentNullException($"Transaction with {id} not found");
                    }
                    
                    response.Result = new TransactionDto(transaction);
                    
                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);

                    response.AddError(CommonErrors.CommonError(ex.Message));

                    return Results.Ok(response);
                }
            })
            .WithName("GetTransactionsById")
            .WithDescription("Получить транзакцию по Id")
            .WithTags(Constants.Groups.Transactions)
            .RequireAuthorization("AdminPolicy")
            .WithOpenApi();

        return Task.CompletedTask;
    }
}