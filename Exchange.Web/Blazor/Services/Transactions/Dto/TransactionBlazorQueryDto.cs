using Exchange.Domain.Enums;

namespace Exchange.Web.Blazor.Services.Transactions.Dto;

public class TransactionBlazorQueryDto
{
    public int? Count { get; set; }
    public int? Offset { get; set; }
    public Guid? UserId { get; set; }
    public Guid? TransactionId { get; set; }
    public IEnumerable<TransactionState>? States { get; set; }
    public DateTime? CreatedDateFrom { get; set; }
    public DateTime? CreatedDateTo { get; set; }
}