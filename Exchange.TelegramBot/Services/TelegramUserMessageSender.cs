using Exchange.Domain.Interfaces;
using Exchange.TelegramBot.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Exchange.TelegramBot.Services;

internal sealed class TelegramUserMessageSender : ITelegramUserMessageSender
{
    private readonly ITelegramBotClient _telegramBotClient;

    public TelegramUserMessageSender(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }

    public async Task SendMessageAsync(
        string message,
        long chatId,
        CancellationToken cancellationToken = default
    )
    {
        var inlineKeyboard = new InlineKeyboardMarkup(
        [
            [
                InlineKeyboardButton.WithUrl("☎️ Связаться с нами", "https://t.me/CryptoTrans_SUPPORT")
            ]
        ]);
        
        await _telegramBotClient.SendMessage(
            chatId: chatId,
            text: message,
            parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
    }
}