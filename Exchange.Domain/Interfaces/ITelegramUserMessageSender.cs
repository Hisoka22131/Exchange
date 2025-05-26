namespace Exchange.Domain.Interfaces;

public interface ITelegramUserMessageSender
{
    Task SendMessageAsync(
        string message,
        long chatId,
        CancellationToken cancellationToken = default);
}