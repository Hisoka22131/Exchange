using Exchange.Common.Enums;

namespace Exchange.Domain.Interfaces;

public interface IRateService
{
    /// <summary>
    /// Получить курс обмена
    /// </summary>
    Task<decimal> CalculateRateAsync(Currency source, Currency target, CancellationToken ct = default);
}