using Exchange.Domain.Enums;

namespace Exchange.Web.Endpoints.Admin.Transactions.UpdateState;

public class UpdateStateRequest
{
    public TransactionState Status { get; set; }
}