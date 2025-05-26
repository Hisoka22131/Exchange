using System.Text.Json.Serialization;
using Exchange.Common.Enums;

namespace Exchange.Web.Blazor.Services.Commissions.Dto;

public class CommissionBlazorDto
{
    public CommissionBlazorDto(Guid id, Currency currency, decimal? amountFrom, decimal? amountTo, decimal? fixedFee, decimal percentFee)
    {
        Id = id;
        Currency = currency;
        AmountFrom = amountFrom;
        AmountTo = amountTo;
        FixedFee = fixedFee;
        PercentFee = percentFee;
    }

    public Guid Id { get;}
    
    [JsonConverter(typeof(JsonStringEnumConverter<Currency>))]
    public Currency Currency { get; set; }
    public decimal? AmountFrom { get; set; }
    public decimal? AmountTo { get; set; }
    public decimal? FixedFee { get; set; }
    public decimal PercentFee { get; set; }
}