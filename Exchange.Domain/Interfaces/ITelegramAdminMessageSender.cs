namespace Exchange.Domain.Interfaces;

public interface ITelegramAdminMessageSender
{
    Task SendMessageAsync(
        string message,
        Guid transactionId,
        bool useCallbackData,
        CancellationToken cancellationToken = default);
}