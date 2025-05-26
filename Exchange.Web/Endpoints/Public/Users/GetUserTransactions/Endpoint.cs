using Exchange.Common.OperationResult;
using Exchange.Common.Resources;
using Exchange.Core.Mediator.Transactions.Query.GetAll;
using Exchange.Core.Mediator.Users.Query.GetLastUserTransactions;
using Exchange.Core.Pagination;
using Exchange.Web.Dto;
using MediatR;

namespace Exchange.Web.Endpoints.Public.Users.GetUserTransactions;

internal sealed class Endpoint : IEndpoint
{
    private const string Path = $"/api/v1/{Constants.Groups.Users}";

    public Task MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet(Path + "/{telegramUserName}", async (
                [FromRoute] string telegramUserName,
                IMediator mediator,
                CancellationToken cancellationToken = default
            ) =>
            {
                var response = new OperationResult<PagedResult<TransactionDto>>();

                try
                {
                    var query = new GetLastUserTransactionsQuery(
                        TelegramUserName: telegramUserName,
                        Count: 5);
                    
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
            .WithName("GetUserTransactions")
            .WithDescription("Получить транзакции пользователя")
            .WithTags(Constants.Groups.Users)
            .WithOpenApi();

        return Task.CompletedTask;
    }
}