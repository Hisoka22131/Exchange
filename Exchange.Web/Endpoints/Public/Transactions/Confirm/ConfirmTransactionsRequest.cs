using Exchange.Common.Enums;
using Exchange.Core.Mediator.Transactions.Commands.Processing;
using Exchange.Domain.Enums;

namespace Exchange.Web.Endpoints.Transactions.Confirm;

public record ConfirmTransactionsRequest(
    Guid TransactionId,
    Currency CurrencyFrom,
    Currency CurrencyTo
)
{
    public ProcessingTransactionCommand MapToCommand()
    {
        return new ProcessingTransactionCommand(
            TransactionId: TransactionId,
            CurrencyFrom: CurrencyFrom,
            CurrencyTo: CurrencyTo);
    }
};