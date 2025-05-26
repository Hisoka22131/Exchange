namespace Exchange.Domain.Interfaces;

public interface IMessageHandler
{
    Task HandleMessageAsync(string message, CancellationToken ct = default);
}