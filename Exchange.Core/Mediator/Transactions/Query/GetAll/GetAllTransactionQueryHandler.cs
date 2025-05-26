using Exchange.Core.Pagination;
using Exchange.Core.Repositories;
using Exchange.Domain.Entities;
using MediatR;

namespace Exchange.Core.Mediator.Transactions.Query.GetAll;

internal sealed class GetAllTransactionQueryHandler : IRequestHandler<GetAllTransactionQuery, PagedResult<TransactionEntity>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllTransactionQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResult<TransactionEntity>> Handle(GetAllTransactionQuery request, CancellationToken cancellationToken)
    {
        var filter = new TransactionsFilter
        {
            Count = request.Count,
            Offset = request.Offset,
            UserId = request.UserId,
            TransactionId = request.TransactionId,
            IncludeUsers = request.IncludeUsers.GetValueOrDefault(),
            States = request.States,
            CreatedDateFrom = request.CreatedDateFrom,
            CreatedDateTo = request.CreatedDateTo
        };
                    
        return await _unitOfWork.Transactions.GetAllAsync(filter, cancellationToken);
    }
}