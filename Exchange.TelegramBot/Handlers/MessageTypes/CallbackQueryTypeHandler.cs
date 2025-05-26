using Exchange.TelegramBot.Commands;
using Exchange.TelegramBot.Interfaces;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Exchange.TelegramBot.Handlers.MessageTypes;

internal sealed class CallbackQueryTypeHandler : ITelegramMessagesHandler
{
    private readonly CommandDispatcher _commandDispatcher;

    public CallbackQueryTypeHandler(CommandDispatcher commandDispatcher)
    {
        _commandDispatcher = commandDispatcher;
    }

    public bool CanProcess(UpdateType updateType)
    {
        return updateType == UpdateType.CallbackQuery;
    }

    public async Task HandleMessageAsync(Update update, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(update.CallbackQuery?.Data))
            return;
        
        var command = update.CallbackQuery.Data.ToLower();
        
        await _commandDispatcher.HandleAsync(update, command, ct);
    }
}