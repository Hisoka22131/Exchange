using Exchange.TelegramBot.Commands;
using Exchange.TelegramBot.Interfaces;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Exchange.TelegramBot.Handlers.MessageTypes;

internal sealed class MessageTypeHandler : ITelegramMessagesHandler
{
    private readonly CommandDispatcher _commandDispatcher;

    public MessageTypeHandler(CommandDispatcher commandDispatcher)
    {
        _commandDispatcher = commandDispatcher;
    }

    public bool CanProcess(UpdateType updateType)
    {
        return updateType == UpdateType.Message;
    }

    public async Task HandleMessageAsync(Update update, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(update.Message?.Text))
            return;

        var command = update.Message.Text.ToLower();
        
        await _commandDispatcher.HandleAsync(update, command, ct);
    }
}