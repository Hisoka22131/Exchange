using Exchange.TelegramBot.Models;

namespace Exchange.TelegramBot.Commands;

public interface ITelegramCommand
{
    bool CanProcess(string command);
    
    Task ProcessingAsync(TelegramUser user, CancellationToken ct = default);
}