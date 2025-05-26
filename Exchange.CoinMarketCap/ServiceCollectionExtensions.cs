using Exchange.CoinMarketCap.Hosted;
using Exchange.CoinMarketCap.Options;
using Exchange.CoinMarketCap.Repositories;
using Exchange.CoinMarketCap.Services;
using Exchange.Core.Repositories;
using Exchange.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.CoinMarketCap;

public static class ServiceCollectionExtensions
{
    public static void AddCoinMarketCapServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddCoinMarketCapHttpClient(configuration)
            .AddApbHttpClient(configuration)
            .AddServices()
            .AddRepositories()
            .AddHostedServices();
    }

    private static IServiceCollection AddCoinMarketCapHttpClient(this IServiceCollection services,
        IConfiguration configuration)
    {
        var configurationSection = configuration
            .GetRequiredSection(CoinMarketCapOptions.SectionName);

        if (configurationSection is null)
            throw new ArgumentNullException(nameof(configurationSection));
        
        services
            .AddOptions<CoinMarketCapOptions>()
            .Bind(configurationSection);
        
        var options = configurationSection.Get<CoinMarketCapOptions>()!;
        
        services.AddHttpClient(CoinMarketCapOptions.HttpClientName,
            settings =>
            {
                settings.BaseAddress = options.Url;
                settings.Timeout = options.Timeout;
                settings.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", options.ApiKey);
            });

        return services;
    }

    private static IServiceCollection AddApbHttpClient(this IServiceCollection services,
        IConfiguration configuration)
    {
        var options = configuration
            .GetRequiredSection(ApbOptions.SectionName)
            .Get<ApbOptions>()!;

        services.AddHttpClient(ApbOptions.HttpClientName,
            settings =>
            {
                settings.BaseAddress = options.Url;
                settings.Timeout = options.Timeout;
            });

        return services;
    }

    private static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        return services
            // .AddHostedService<CheckCreditsService>()
            .AddHostedService<UpdateCmcCurrenciesService>();
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
                .AddTransient<ICreditsService, CoinMarketCapService>()
            ;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddKeyedTransient<ICurrenciesRepository, CoinMarketCapCurrenciesRepository>(
                "CoinMarketCapCurrenciesRepository");
    }
}