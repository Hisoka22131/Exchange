using Exchange.Domain.Enums;
using MediatR;

namespace Exchange.Core.Mediator.Transactions.Commands.Complete;

public record CompleteTransactionCommand(
    Guid TransactionId,
    TransactionState TransactionState
) : IRequest;