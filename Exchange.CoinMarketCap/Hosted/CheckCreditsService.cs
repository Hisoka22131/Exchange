using Exchange.CoinMarketCap.Options;
using Exchange.Domain.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Exchange.CoinMarketCap.Hosted;

internal sealed class CheckCreditsService : BackgroundService
{
    private readonly ICreditsService _creditsService;
    private readonly CoinMarketCapOptions _marketCapOptions;

    public CheckCreditsService(
        ICreditsService creditsService,
        IOptions<CoinMarketCapOptions> marketCapOptions
    )
    {
        _creditsService = creditsService;
        _marketCapOptions = marketCapOptions.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var delayTime = _marketCapOptions.DelayBetweenReceivingCredits;

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckCreditsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{nameof(CheckCreditsService)} throw an exception: {ex}");
            }

            await Task.Delay(delayTime, stoppingToken);
        }
    }

    private async Task CheckCreditsAsync(CancellationToken stoppingToken)
    {
        // TODO: отправить в телеграм уведомление если осталось <= 1000

        var creditsCount = await _creditsService.CalculateCreditsAsync(stoppingToken);
    }
}