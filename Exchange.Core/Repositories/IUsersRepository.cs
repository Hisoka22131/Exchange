using Exchange.Core.Pagination;
using Exchange.Domain.Entities;

namespace Exchange.Core.Repositories;

public interface IUsersRepository
{
    Task AddAsync(
        UserEntity user, 
        CancellationToken cancellationToken = default);
    
    Task<UserEntity?> GetByIdAsync(
        Guid id, 
        CancellationToken cancellationToken = default);
    
    Task<UserEntity?> GetByTelegramCredentialsAsync(
        string telegramUserName, 
        CancellationToken cancellationToken = default);
    
    Task<PagedResult<UserEntity>> GetAllAsync(
        UsersFilter filter,
        CancellationToken cancellationToken = default);
    
    void Update(UserEntity user);
}

public class UsersFilter
{
    public int? Count { get; set; }
    public int? Offset { get; set; }
    public long? TelegramUserId { get; set; }
    public string? TelegramUserName { get; set; }
    public bool IncludeTransactions { get; set; }
    public Guid? UserId { get; set; }
}