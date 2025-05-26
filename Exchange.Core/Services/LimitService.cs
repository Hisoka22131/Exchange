using Exchange.Common.Enums;
using Exchange.Domain.Enums;
using Exchange.Domain.Interfaces;

namespace Exchange.Core.Services;

internal sealed class LimitService : ILimitService
{
    private const decimal Tolerance = 0.0001m;
    
    private readonly LimitInfo _limitInfo;
    private readonly IRateService _rateService;

    public LimitService(IRateService rateService)
    {
        _rateService = rateService;
        
        _limitInfo = new LimitInfo
        {
            Min = 50m,
            Max = 10_000m,
            Currency = Currency.USDT
        };
    }

    public async Task<(decimal Min, decimal Max)> GetLimitAsync(Currency currency, CancellationToken cancellationToken)
    {
        if (currency == _limitInfo.Currency)
        {
            // Если валюта - базовая (USDT), возвращаем фиксированные значения
            return (_limitInfo.Min, _limitInfo.Max);
        }

        var rate = await _rateService.CalculateRateAsync(_limitInfo.Currency, currency, cancellationToken);

        // Рассчитываем эквиваленты в запрашиваемой валюте
        var minInCurrency = _limitInfo.Min * rate;
        var maxInCurrency = _limitInfo.Max * rate;

        // Пересчет обратно в USDT (для проверки точности)
        var minBackToUsdt = minInCurrency / rate;
        var maxBackToUsdt = maxInCurrency / rate;

        // Проверяем отклонения
        if (Math.Abs(minBackToUsdt - _limitInfo.Min) > Tolerance ||
            Math.Abs(maxBackToUsdt - _limitInfo.Max) > Tolerance)
        {
            throw new InvalidOperationException("Расчеты лимитов несовместимы с базовыми значениями.");
        }
        
        return (minInCurrency, maxInCurrency);
    }
}

//TODO: в будущем вынести в базу
public class LimitInfo
{
    public decimal Min { get; set; }
    public decimal Max { get; set; }
    public Currency Currency { get; set; }
}