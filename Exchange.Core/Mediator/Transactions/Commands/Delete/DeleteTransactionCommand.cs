using MediatR;

namespace Exchange.Core.Mediator.Transactions.Commands.Delete;

public record DeleteTransactionCommand : IRequest;