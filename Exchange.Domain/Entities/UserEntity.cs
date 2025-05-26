namespace Exchange.Domain.Entities;

public class UserEntity
{
    public UserEntity(long telegramUserId, string telegramUserName)
    {
        Id = Guid.NewGuid();
        TelegramUserId = telegramUserId;
        TelegramUserName = telegramUserName;
        Transactions = new List<TransactionEntity>();
    }

    public Guid Id { get; }
    public long TelegramUserId { get; init; }
    public string TelegramUserName { get; init; }
    public ICollection<TransactionEntity>? Transactions { get; }
}