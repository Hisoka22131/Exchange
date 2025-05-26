using Exchange.Common.Enums;

namespace Exchange.Domain.Interfaces;

public interface ICommissionService
{
    Task<(decimal Fee, decimal Percent)> CalculateFeeInUsdtAsync(
        decimal amount, 
        Currency currency, 
        CancellationToken ct = default);
}