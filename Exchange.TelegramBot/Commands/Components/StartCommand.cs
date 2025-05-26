using Exchange.TelegramBot.Models;
using Exchange.TelegramBot.Options;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Exchange.TelegramBot.Commands.Components;

internal sealed class StartCommand : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly TelegramBotOptions _telegramBotOptions;
    
    public StartCommand(ITelegramBotClient telegramBotClient, IOptions<TelegramBotOptions> options)
    {
        _telegramBotClient = telegramBotClient;
        _telegramBotOptions = options.Value;
    }

    public bool CanProcess(string command)
    {
        return command.StartsWith(CommandsConstants.Common.Start);
    }

    public async Task ProcessingAsync(TelegramUser user, CancellationToken ct = default)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(
        [
            [
                InlineKeyboardButton.WithWebApp("📺 Открыть приложение", _telegramBotOptions.WebAppUrl)
            ],
            [
                InlineKeyboardButton.WithCallbackData("👤 Профиль", CommandsConstants.User.GetMyProfile)
            ],
            [
                InlineKeyboardButton.WithUrl("☎️ Контакты", "https://t.me/CryptoTrans_SUPPORT")
            ],
            [
                InlineKeyboardButton.WithUrl("🌍 Новостной канал", "https://t.me/CryptoTrans_NEWS")
            ]
        ]);

        const string stickerId = "CAACAgIAAxkBAAPRZ1Q65umG4tPrReGxCzf5brMXd2wAAi4AAyRxYhqI6DZDakBDFDYE";

        await _telegramBotClient.SendSticker(
            chatId: user.ChatId,
            sticker: InputFile.FromFileId(stickerId),
            cancellationToken: ct);
        
        var message = await _telegramBotClient.SendMessage(
            chatId: user.ChatId,
            text: $"Добро пожаловать, @{user.Username}!\nВыберите раздел:",
            replyMarkup: inlineKeyboard,
            cancellationToken: ct);

        Task.Run(async () => { await DeleteMessageAsync(user.ChatId, message.MessageId - 2, ct); }, ct);
    }
    
    private async Task DeleteMessageAsync(long chatId, int messageId, CancellationToken ct = default)
    {
        var j = 0;
        for (var i = messageId; i > 0; i--)
        {
            try
            {
                await _telegramBotClient
                    .DeleteMessage(chatId, i, cancellationToken: ct);
            }
            catch (Exception ex)
            {
                // Console.WriteLine($"{ex.Message} - {i}");
                j++;

                if (j < 5) continue;
                break;
            }
        }
    }
}