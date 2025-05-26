using Exchange.Domain.Interfaces;
using Exchange.TelegramBot.Commands;
using Exchange.TelegramBot.Commands.Components;
using Exchange.TelegramBot.Handlers;
using Exchange.TelegramBot.Handlers.MessageTypes;
using Exchange.TelegramBot.Interfaces;
using Exchange.TelegramBot.Options;
using Exchange.TelegramBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace Exchange.TelegramBot;

public static class ServiceCollectionExtensions
{
    public static void AddTelegramBotServices(this IServiceCollection services, IConfiguration configuration)
    {
        var configurationSection = configuration.GetRequiredSection(TelegramBotOptions.SectionName);
        
        if (configurationSection is null)
            throw new ArgumentNullException(nameof(configurationSection));

        var options = configurationSection.Get<TelegramBotOptions>()!;
        
        if (!options.Enabled)
        {
            Console.WriteLine("TelegramBot is disabled");
            return;
        }
        
        services
            .AddOptions<TelegramBotOptions>()
            .Bind(configurationSection);
        
        services.AddScoped<IMessageHandler, TelegramBotMessageHandler>();
        
        var telegramBotClient = new TelegramBotClient(options.Token);
        
        var url = options.ServerUrl.Trim() + "/api/telegram/message";
        telegramBotClient.SetWebhook(url).Wait();
        
        services.AddSingleton<ITelegramBotClient>(telegramBotClient);
        
        services.AddHandlers();
        services.AddCommands();
        services.AddServices();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services
            .AddTransient<ITelegramAdminMessageSender, AdminChatMessageSender>()
            .AddTransient<ITelegramUserMessageSender, TelegramUserMessageSender>();
    }
    
    private static void AddHandlers(this IServiceCollection services)
    {
        services
            .AddTransient<ITelegramMessagesHandler, MessageTypeHandler>()
            .AddTransient<ITelegramMessagesHandler, CallbackQueryTypeHandler>();
    }
    
    private static void AddCommands(this IServiceCollection services)
    {
        services
            .AddSingleton<CommandDispatcher>()
            .AddTransient<ITelegramCommand, StartCommand>()
            .AddTransient<ITelegramCommand, HelpCommand>()
            .AddTransient<ITelegramCommand, GetContactsCommand>()
            .AddTransient<ITelegramCommand, ConfirmTransactionCommand>()
            .AddTransient<ITelegramCommand, RejectTransactionCommand>()
            .AddTransient<ITelegramCommand, GetChatIdCommand>()
            .AddTransient<ITelegramCommand, GetMyProfileCommand>()
            ;
    }
}