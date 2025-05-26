using Exchange.Common.Enums;
using Exchange.Domain.Enums;

namespace Exchange.Web.Endpoints.Public.Exchange.Course;

public record PostCourseRequest
{
    public required Currency CurrencyFrom { get; init; }
    public decimal? AmountFrom { get; init; }

    public required Currency CurrencyTo { get; init; }
    public decimal? AmountTo { get; init; }
}