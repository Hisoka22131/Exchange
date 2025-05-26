using Exchange.Core.Repositories;
using Exchange.Domain.Entities;
using MediatR;

namespace Exchange.Core.Mediator.Transactions.Query.GetById;

internal sealed class GetTransactionQueryHandler : IRequestHandler<GetTransactionQuery, TransactionEntity?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTransactionQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TransactionEntity?> Handle(GetTransactionQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Transactions.GetByIdAsync(request.Id, cancellationToken);
    }
}