using Exchange.CoinMarketCap.Options;
using Exchange.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Exchange.CoinMarketCap.Hosted;

internal sealed class UpdateCmcCurrenciesService : BackgroundService
{
    private readonly ICurrenciesRepository _currenciesRepository;
    private readonly CoinMarketCapOptions _marketCapOptions;

    public UpdateCmcCurrenciesService(
        [FromKeyedServices("CoinMarketCapCurrenciesRepository")]
        ICurrenciesRepository currenciesRepository,
        IOptions<CoinMarketCapOptions> options
    )
    {
        _currenciesRepository = currenciesRepository;
        _marketCapOptions = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var delayTime = _marketCapOptions.DelayBetweenReceivingCurrency;

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await UpdateCurrenciesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{nameof(UpdateCmcCurrenciesService)} throw an exception: {ex}");
            }

            await Task.Delay(delayTime, stoppingToken);
        }
    }

    private async Task UpdateCurrenciesAsync(CancellationToken stoppingToken)
    {
        await _currenciesRepository.UpdateCurrenciesAsync(stoppingToken);
    }
}