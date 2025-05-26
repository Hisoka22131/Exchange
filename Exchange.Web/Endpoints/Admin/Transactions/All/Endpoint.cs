using Exchange.Common.OperationResult;
using Exchange.Common.Resources;
using Exchange.Core.Mediator.Transactions.Query.GetAll;
using Exchange.Core.Pagination;
using Exchange.Web.Dto;
using MediatR;

namespace Exchange.Web.Endpoints.Admin.Transactions.All;

internal sealed class Endpoint : IEndpoint
{
    private const string Path = $"/api/v1/{Constants.Groups.Transactions}";

    public Task MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet(Path, async (
                [FromQuery] int? count,
                [FromQuery] int? offset,
                [FromQuery] Guid? userId,
                [FromQuery] Guid? transactionId,
                [FromQuery] string? states,
                [FromQuery] DateTimeOffset? createdDateFrom,
                [FromQuery] DateTimeOffset? createdDateTo,
                IMediator mediator,
                CancellationToken cancellationToken = default
            ) =>
            {
                var response = new OperationResult<PagedResult<TransactionDto>>();

                try
                {
                    var statesArray = states?.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    
                    var query = new GetAllTransactionQuery(
                        Count: count,
                        Offset: offset,
                        UserId: userId,
                        TransactionId: transactionId,
                        IncludeUsers: false,
                        States: statesArray,
                        CreatedDateFrom: createdDateFrom,
                        CreatedDateTo: createdDateTo);
                    
                    var transactionsPage = await mediator.Send(query, cancellationToken);
                    
                    response.Result = new PagedResult<TransactionDto>
                    {
                        CurrentPage = transactionsPage.CurrentPage,     
                        PageSize = transactionsPage.PageSize,     
                        TotalCount = transactionsPage.TotalCount,     
                        Items = transactionsPage.Items.Select(x => new TransactionDto(x)) 
                    };

                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);

                    response.AddError(CommonErrors.CommonError(ex.Message));

                    return Results.Ok(response);
                }
            })
            .WithName("GetTransactionsByFilter")
            .WithDescription("Получить список транзакций по фильтру")
            .WithTags(Constants.Groups.Transactions)
            .RequireAuthorization("AdminPolicy")
            .WithOpenApi();

        return Task.CompletedTask;
    }
}