using Exchange.Common.Enums;
using Exchange.Domain.Enums;
using MediatR;

namespace Exchange.Core.Mediator.Course.Query;

public record GetCourseQuery(
    Currency FromCurrency,
    decimal? FromAmount,
    Currency ToCurrency,
    decimal? ToAmount
) : IRequest<Domain.Entities.Course>;