using Blazored.SessionStorage;
using Blazored.Toast;
using Exchange.Web.Blazor.Auth;
using Exchange.Web.Blazor.Components;
using Exchange.Web.Blazor.Login;
using Exchange.Web.Blazor.Options;
using Exchange.Web.Blazor.Services.Commissions;
using Exchange.Web.Blazor.Services.Transactions;
using Exchange.Web.Blazor.Services.Users;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

namespace Exchange.Web.Blazor;

public static class ServiceCollectionExtensions
{
    public static void AddBlazorServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
        services.AddBlazoredSessionStorage();

        services
            .AddMudServices()
            .AddBlazoredToast()
            .AddRazorComponents()
            .AddInteractiveServerComponents();

        var configurationSection = configuration.GetRequiredSection(BlazorOptions.SectionName);

        if (configurationSection is null)
            throw new ArgumentNullException(nameof(configurationSection));

        var options = configurationSection.Get<BlazorOptions>()!;
        
        services.AddHttpClient(BlazorOptions.ClientName, client =>
        {
            client.BaseAddress = options.Url;
            client.Timeout = options.Timeout;
            client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "skip-browser-warning");
        });

        services.AddScoped<TransactionsService>();
        services.AddScoped<UsersService>();
        services.AddScoped<CommissionsService>();
        services.AddScoped<LoginService>();
    }

    public static void UseBlazorServices(this WebApplication app)
    {
        app
            .MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();
    }
}