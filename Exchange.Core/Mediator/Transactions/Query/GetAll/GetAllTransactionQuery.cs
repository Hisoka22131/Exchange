using Exchange.Core.Pagination;
using Exchange.Domain.Entities;
using MediatR;

namespace Exchange.Core.Mediator.Transactions.Query.GetAll;

public record GetAllTransactionQuery(
    int? Count,
    int? Offset,
    Guid? UserId,
    Guid? TransactionId,
    bool? IncludeUsers,
    string[]? States,
    DateTimeOffset? CreatedDateFrom,
    DateTimeOffset? CreatedDateTo
) : IRequest<PagedResult<TransactionEntity>>;