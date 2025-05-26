using Exchange.TelegramBot.Extensions;
using Exchange.TelegramBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Exchange.TelegramBot.Commands.Components;

internal sealed class HelpCommand : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBotClient;

    public HelpCommand(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }

    public bool CanProcess(string command)
    {
        return command.StartsWith(CommandsConstants.Common.Help);
    }

    public async Task ProcessingAsync(TelegramUser user, CancellationToken ct = default)
    {
        var inlineKeyboard = InlineKeyboardMarkupExtensions.CreateWithMenu(
            inlineKeyboard:
            [
                [
                    InlineKeyboardButton.WithUrl("Сайт", "https://example.com")
                ]
            ]);

        await _telegramBotClient.SendMessage(
            chatId: user.ChatId,
            text: "Произошла ошибка.\nОбратитесь в поддержку:",
            replyMarkup: inlineKeyboard,
            cancellationToken: ct);
    }
}