using Exchange.Core.Hosted;
using Exchange.Core.Options;
using Exchange.Core.Services;
using Exchange.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Core;

public static class ServiceCollectionExtensions
{
    public static void AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMediatrCore()
            .AddServices()
            .AddMemoryCache()
            .AddPhoneNumbersOptions(configuration)
            .AddPercentOptions(configuration)
            .AddHostedService<ClearTransactionsBackgroundService>();
    }

    private static IServiceCollection AddMediatrCore(this IServiceCollection services)
    {
        return services
            .AddMediatR(cf => cf.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
            .AddTransient<ICommissionService, CommissionService>()
            .AddTransient<ILimitService, LimitService>()
            .AddSingleton<ICacheService, MemoryCacheService>()
            .AddTransient<IRateService, RateService>()
            ;
    }
    
    private static IServiceCollection AddPhoneNumbersOptions(this IServiceCollection services, IConfiguration configuration)
    {
        var configurationSection = configuration.GetRequiredSection(PhoneNumbers.SectionName);
        
        if (configurationSection is null)
            throw new ArgumentNullException(nameof(configurationSection));
        
        services
            .AddOptions<PhoneNumbers>()
            .Bind(configurationSection);

        return services;
    }
    
    private static IServiceCollection AddPercentOptions(this IServiceCollection services, IConfiguration configuration)
    {
        var configurationSection = configuration.GetRequiredSection(PercentOptions.SectionName);
        
        if (configurationSection is null)
            throw new ArgumentNullException(nameof(configurationSection));
        
        services
            .AddOptions<PercentOptions>()
            .Bind(configurationSection);
        
        return services;
    }
}