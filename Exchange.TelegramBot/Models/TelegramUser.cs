namespace Exchange.TelegramBot.Models;

public record TelegramUser(
    long ChatId,
    string Username,
    string Command,
    int? MessageId
);