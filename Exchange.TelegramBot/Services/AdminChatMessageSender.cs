using Exchange.Domain.Interfaces;
using Exchange.TelegramBot.Options;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Exchange.TelegramBot.Services;

internal sealed class AdminChatMessageSender : ITelegramAdminMessageSender
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly long _chatId;

    public AdminChatMessageSender(ITelegramBotClient telegramBotClient, IOptions<TelegramBotOptions> options)
    {
        _telegramBotClient = telegramBotClient;
        _chatId = options.Value.AdminChatId;
    }

    public async Task SendMessageAsync(
        string message,
        Guid transactionId,
        bool useCallbackData,
        CancellationToken cancellationToken = default
    )
    {
        IReplyMarkup? inlineKeyboard = null;

        if (useCallbackData)
        {
            inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("✅ Подтвердить", $"/confirm:{transactionId}"),
                    InlineKeyboardButton.WithCallbackData("❌ Отклонить", $"/reject:{transactionId}")
                }
            });
        }
        
        await _telegramBotClient.SendMessage(
            chatId: _chatId,
            text: message,
            parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
    }
}