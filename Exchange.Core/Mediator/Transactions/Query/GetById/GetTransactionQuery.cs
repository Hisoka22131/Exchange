using Exchange.Domain.Entities;
using MediatR;

namespace Exchange.Core.Mediator.Transactions.Query.GetById;

public record GetTransactionQuery(Guid Id) : IRequest<TransactionEntity?>;