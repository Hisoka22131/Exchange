using Exchange.Common.Enums;

namespace Exchange.Domain.Interfaces;

public interface ILimitService
{
    /// <summary>
    /// Получаем лимиты в монете отправления
    /// Например:
    /// У нас лимит в базе от 50 до 10_000 USDT
    /// Currency = BTC
    /// Метод конвертирует BTC в USDT и вычисляем лимит в BTC!
    /// </summary>
    Task<(decimal Min, decimal Max)> GetLimitAsync(Currency currency, CancellationToken cancellationToken);
}