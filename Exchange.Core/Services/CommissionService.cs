using Exchange.Common.Enums;
using Exchange.Core.Repositories;
using Exchange.Domain.Enums;
using Exchange.Domain.Interfaces;

namespace Exchange.Core.Services;

internal sealed class CommissionService : ICommissionService
{
    private readonly IRateService _rateService;
    private readonly ICommissionRepository _commissionRepository;

    public CommissionService(IRateService rateService, ICommissionRepository commissionRepository)
    {
        _rateService = rateService;
        _commissionRepository = commissionRepository;
    }
    
    public async Task<(decimal Fee, decimal Percent)> CalculateFeeInUsdtAsync(decimal amount, Currency currency, CancellationToken ct = default)
    {
        var amountInUsdt = await ConvertFromUsdtAsync(amount, currency.GetCurrency(), ct);

        var commission = await _commissionRepository.GetAsync(amountInUsdt, ct);
        
        if (commission == null)
        {
            throw new InvalidOperationException("Комиссия для указанной суммы не найдена.");
        }
        
        var fee = commission.FixedFee ?? amountInUsdt * commission.PercentFee;

        return (fee, commission.PercentFee);
    }
    
    private async Task<decimal> ConvertFromUsdtAsync(decimal amount, Currency currency, CancellationToken ct)
    {
        if (currency == Currency.USDT)
        {
            return amount;
        }

        var rate = await _rateService.CalculateRateAsync(currency.GetCurrency(), Currency.USDT, ct);

        return amount * rate;
    }
}