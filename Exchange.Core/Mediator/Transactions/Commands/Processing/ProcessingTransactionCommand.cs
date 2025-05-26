using Exchange.Common.Enums;
using Exchange.Domain.Enums;
using MediatR;

namespace Exchange.Core.Mediator.Transactions.Commands.Processing;

public record ProcessingTransactionCommand(
    Guid TransactionId,
    Currency CurrencyFrom,
    Currency CurrencyTo
) : IRequest;