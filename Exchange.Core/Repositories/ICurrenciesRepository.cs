using Exchange.Domain.Entities;

namespace Exchange.Core.Repositories;

public interface ICurrenciesRepository
{
    Task<CurrencyInfo> GetAllCurrenciesAsync(CancellationToken ct = default);
    Task UpdateCurrenciesAsync(CancellationToken ct = default);
}