using Exchange.Core.Pagination;
using Exchange.Core.Repositories;
using Exchange.Database.Context;
using Exchange.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Exchange.Database.Repositories;

internal sealed class UsersRepository : IUsersRepository
{
    private readonly ExchangeDbContext _context;

    public UsersRepository(ExchangeDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(UserEntity user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
    }
    
    public void Update(UserEntity user)
    {
        _context.Users.Update(user);
    }

    public async Task<UserEntity?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.Users.FindAsync([id], cancellationToken);
    }

    public async Task<UserEntity?> GetByTelegramCredentialsAsync(
        string telegramUserName,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.TelegramUserName.Equals(telegramUserName), cancellationToken);
    }

    public async Task<PagedResult<UserEntity>> GetAllAsync(UsersFilter filter,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Users.AsQueryable();

        query = Filter(query, filter);

        var totalCount = await query.CountAsync(cancellationToken);

        if (filter.Offset.HasValue)
        {
            query = query.Skip(filter.Offset.Value);
        }

        if (filter.Count.HasValue)
        {
            query = query.Take(filter.Count.Value);
        }

        var items = await query.ToListAsync(cancellationToken);

        return new PagedResult<UserEntity>
        {
            Items = items,
            TotalCount = totalCount,
            PageSize = filter.Count ?? totalCount,
            CurrentPage = filter is { Offset: not null, Count: not null }
                ? filter.Offset.Value / filter.Count.Value + 1
                : 1
        };
    }

    private static IQueryable<UserEntity> Filter(IQueryable<UserEntity> query, UsersFilter filter)
    {
        if (filter.IncludeTransactions)
        {
            query = query.Include(x => x.Transactions);
        }

        if (filter.TelegramUserId.HasValue)
        {
            query = query.Where(u => u.TelegramUserId == filter.TelegramUserId.Value);
        }
        
        if (filter.UserId.HasValue)
        {
            query = query.Where(u => u.Id == filter.UserId.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.TelegramUserName))
        {
            query = query.Where(u => u.TelegramUserName.Equals(filter.TelegramUserName));
        }

        return query;
    }
}