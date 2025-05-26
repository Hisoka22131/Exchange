using Exchange.Common.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Common;

public static class ServiceCollectionExtensions
{
    public static void AddHttpLoggingHandler(this IServiceCollection services)
    {
        services.AddTransient<HttpLoggingHandler>();
    }
}