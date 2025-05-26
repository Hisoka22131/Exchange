using Exchange.Domain.Enums;
using MediatR;

namespace Exchange.Core.Mediator.Transactions.Commands.UpdateState;

public record UpdateStateCommand(Guid Id, TransactionState State) : IRequest;