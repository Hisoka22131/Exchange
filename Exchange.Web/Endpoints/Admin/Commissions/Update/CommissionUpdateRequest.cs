using Exchange.Common.Enums;
using Exchange.Domain.Entities;

namespace Exchange.Web.Endpoints.Admin.Commissions.Update;

public class CommissionUpdateRequest
{
    public Guid Id { get; set; }
    public Currency Currency { get; set; }
    public decimal AmountFrom { get; set; }
    public decimal? AmountTo { get; set; }
    public decimal? FixedFee { get; set; }
    public decimal PercentFee { get; set; }

    public CommissionInfoEntity MapToEntity()
    {
        return new CommissionInfoEntity
        {
            Id = Id,
            Currency = Currency,
            AmountFrom = AmountFrom,
            AmountTo = AmountTo,
            FixedFee = FixedFee,
            PercentFee = PercentFee
        };
    }
}