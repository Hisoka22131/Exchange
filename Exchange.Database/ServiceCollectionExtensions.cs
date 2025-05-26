using Exchange.Core.Repositories;
using Exchange.Database.Context;
using Exchange.Database.Options;
using Exchange.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Database;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddRepositories()
            .AddDatabase(configuration);
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<ICommissionRepository, CommissionsRepository>()
            .AddScoped<IUsersRepository, UsersRepository>()
            .AddScoped<ITransactionsRepository, TransactionsRepository>()
            ;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var configurationSection = configuration.GetRequiredSection(ExchangeDatabaseOptions.SectionName);

        if (configurationSection is null)
            throw new ArgumentNullException(nameof(configurationSection));
        
        var options = configurationSection.Get<ExchangeDatabaseOptions>()!;

        services
            .AddOptions<ExchangeDatabaseOptions>()
            .Bind(configurationSection);
        
        services.AddDbContext<ExchangeDbContext>(optionsBuilder =>
            optionsBuilder.UseNpgsql(options.ConnectionString));
        
        if (options.AutoMigrations)
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ExchangeDbContext>();
            dbContext.Database.Migrate();
        }
        
        return services;
    }
}