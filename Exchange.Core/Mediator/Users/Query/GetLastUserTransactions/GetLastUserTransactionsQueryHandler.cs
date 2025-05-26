using Exchange.Core.Pagination;
using Exchange.Core.Repositories;
using Exchange.Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Core.Mediator.Users.Query.GetLastUserTransactions;

internal sealed class GetLastUserTransactionsQueryHandler : IRequestHandler<GetLastUserTransactionsQuery , PagedResult<TransactionEntity>>
{
    private readonly IServiceProvider _serviceProvider;

    public GetLastUserTransactionsQueryHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task<PagedResult<TransactionEntity>> Handle(GetLastUserTransactionsQuery req, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        var user = await unitOfWork.Users.GetByTelegramCredentialsAsync(
            telegramUserName: req.TelegramUserName,
            cancellationToken: cancellationToken);

        if (user == null)
        {
            return new PagedResult<TransactionEntity>();
        }
        
        var filter = new TransactionsFilter
        {
            Count = req.Count,
            Offset = 0,
            UserId = user.Id,
            TransactionId = null,
            IncludeUsers = false,
            States = null,
            CreatedDateFrom = null,
            CreatedDateTo = null
        };
                    
        return await unitOfWork.Transactions.GetAllAsync(filter, cancellationToken);
    }
}