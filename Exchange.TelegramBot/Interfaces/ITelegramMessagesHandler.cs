using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Exchange.TelegramBot.Interfaces;

public interface ITelegramMessagesHandler
{
    public bool CanProcess(UpdateType updateType);
    Task HandleMessageAsync(Update update, CancellationToken ct = default);
}