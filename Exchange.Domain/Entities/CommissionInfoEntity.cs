using Exchange.Common.Enums;
using Exchange.Domain.Entities.Interfaces;

namespace Exchange.Domain.Entities;

public class CommissionInfoEntity : ICreatable, IUpdatable
{
    public Guid Id { get; set; }
    public Currency Currency { get; set; }
    public decimal AmountFrom { get; set; }
    public decimal? AmountTo { get; set; }
    public decimal? FixedFee { get; set; }
    public decimal PercentFee { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}