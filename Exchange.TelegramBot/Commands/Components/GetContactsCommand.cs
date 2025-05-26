using Exchange.TelegramBot.Extensions;
using Exchange.TelegramBot.Models;
using Exchange.TelegramBot.Options;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Exchange.TelegramBot.Commands.Components;

internal sealed class GetContactsCommand : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly TelegramBotOptions _telegramBotOptions;
    public GetContactsCommand(ITelegramBotClient telegramBotClient, IOptions<TelegramBotOptions> options)
    {
        _telegramBotClient = telegramBotClient;
        _telegramBotOptions = options.Value;
    }

    public bool CanProcess(string command)
    {
        return command.StartsWith(CommandsConstants.Common.GetListOfContacts);
    }

    public async Task ProcessingAsync(TelegramUser user, CancellationToken ct = default)
    {
        var inlineKeyboard = InlineKeyboardMarkupExtensions.CreateWithMenu(
            inlineKeyboard:
            [
                [
                    InlineKeyboardButton.WithUrl("Telegram", "https://t.me/CryptoTrans_SUPPORT")
                ]
            ]);

        await _telegramBotClient.SendMessage(
            chatId: user.ChatId,
            text: "Наши контакты:",
            replyMarkup: inlineKeyboard,
            cancellationToken: ct);
    }
}