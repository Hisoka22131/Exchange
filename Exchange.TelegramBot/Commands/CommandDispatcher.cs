using Exchange.TelegramBot.Models;
using Telegram.Bot.Types;

namespace Exchange.TelegramBot.Commands;

internal class CommandDispatcher
{
    private readonly IEnumerable<ITelegramCommand> _commands;

    public CommandDispatcher(IEnumerable<ITelegramCommand> commands)
    {
        _commands = commands;
    }

    public async Task HandleAsync(Update update, string command, CancellationToken ct = default)
    {
        var commandHandler = _commands.FirstOrDefault(x => x.CanProcess(command));
        
        if (commandHandler is null) return;
        
        var user = new TelegramUser(
            GetChatId(update),
            GetUsername(update),
            command,
            GetMessageId(update)
        );
        
        await commandHandler.ProcessingAsync(user, ct);
    }

    private static long GetChatId(Update update)
    {
        return update.Message?.Chat.Id
               ?? update.CallbackQuery?.From.Id
               ?? update.EditedMessage?.From?.Id
               ?? throw new InvalidOperationException("Unknown chat");
    }

    private static string GetUsername(Update update)
    {
        return update.Message?.Chat?.Username
               ?? update.CallbackQuery?.Message?.Chat.Username
               ?? update.CallbackQuery?.From?.Username
               ?? update.EditedMessage?.Chat?.Username
               ?? update.Message?.From?.Username
               ?? throw new InvalidOperationException("Unknown username");
    }
    
    private static int? GetMessageId(Update update)
    {
        return update.Message?.MessageId
               ?? update.CallbackQuery?.Message?.MessageId;
    }
}