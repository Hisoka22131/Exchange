namespace Exchange.TelegramBot.Options;

public class TelegramBotOptions
{
    public const string SectionName = "TelegramBot";
    
    public required bool Enabled { get; init; }
    public required string Token { get; init; }
    public required string ServerUrl { get; init; }
    public required string WebAppUrl { get; init; }
    public required long AdminChatId { get; init; }
    public required string TelegramContact { get; init; }
}