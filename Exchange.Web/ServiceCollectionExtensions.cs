using Exchange.CoinMarketCap;
using Exchange.Core;
using Exchange.Database;
using Exchange.Excel;
using Exchange.TelegramBot;
using Exchange.Web.Blazor;
using Exchange.Web.Blazor.Components;
using Exchange.Web.Endpoints;
using Exchange.Web.Extensions;

namespace Exchange.Web;

public static class ServiceCollectionExtensions
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddWebServices(configuration);
        services.AddCoreServices(configuration);
        services.AddTelegramBotServices(configuration);
        services.AddDatabaseServices(configuration);
        services.AddCoinMarketCapServices(configuration);
        // services.AddBlockchainServices(configuration);
    }

    public static void UseServices(this WebApplication app)
    {
        app.UseGlobalExceptionHandler();

        app.UseAuthentication();
        app.UseAuthorization();

        // if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //TODO:
        app.UseCors("AllowAll");

        app.UseStaticFiles();

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.UseBlazorServices();

        app.MapEndpoints();
    }
}