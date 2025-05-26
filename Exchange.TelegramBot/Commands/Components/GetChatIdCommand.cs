using Exchange.TelegramBot.Models;
using Telegram.Bot;

namespace Exchange.TelegramBot.Commands.Components;

internal sealed class GetChatIdCommand : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBotClient;

    public GetChatIdCommand(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }

    public bool CanProcess(string command)
    {
        return command.StartsWith(CommandsConstants.Common.GetChatId);
    }

    public async Task ProcessingAsync(TelegramUser user, CancellationToken ct = default)
    {
        await _telegramBotClient.SendMessage(
            chatId: user.ChatId,
            text: $"Идентификатор чата '{user.ChatId}'",
            cancellationToken: ct);
    }
}