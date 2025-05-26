using Exchange.Core.Pagination;
using Exchange.Core.Repositories;
using Exchange.Database.Context;
using Exchange.Domain.Entities;
using Exchange.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Exchange.Database.Repositories;

internal sealed class TransactionsRepository : ITransactionsRepository
{
    private readonly ExchangeDbContext _context;

    public TransactionsRepository(ExchangeDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TransactionEntity transaction, CancellationToken cancellationToken = default)
    {
        await _context.Transactions.AddAsync(transaction, cancellationToken);
    }

    public void Update(TransactionEntity transaction)
    {
        _context.Transactions.Update(transaction);
    }

    public async Task<TransactionEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Transactions
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public void DeleteRange(IEnumerable<TransactionEntity> transactions)
    {
        _context.Transactions.RemoveRange(transactions);
    }

    public async Task<PagedResult<TransactionEntity>> GetAllAsync(
        TransactionsFilter filter,
        CancellationToken cancellationToken = default
    )
    {
        var query = _context.Transactions.AsQueryable();

        query = Filter(query, filter);

        var totalCount = await query.CountAsync(cancellationToken);

        query = query.OrderByDescending(x => x.CreatedAt.UtcDateTime);

        if (filter.Offset.HasValue)
        {
            query = query.Skip(filter.Offset.Value);
        }

        if (filter.Count.HasValue)
        {
            query = query.Take(filter.Count.Value);
        }

        var items = await query
            .ToListAsync(cancellationToken);

        return new PagedResult<TransactionEntity>
        {
            Items = items,
            TotalCount = totalCount,
            PageSize = filter.Count ?? totalCount,
            CurrentPage = filter is { Offset: not null, Count: not null }
                ? filter.Offset.Value / filter.Count.Value + 1
                : 1
        };
    }

    private static IQueryable<TransactionEntity> Filter(
        IQueryable<TransactionEntity> query,
        TransactionsFilter filter)
    {
        if (filter.IncludeUsers)
        {
            query = query.Include(t => t.User);
        }

        if (filter.UserId.HasValue)
        {
            query = query.Where(t => t.User.Id == filter.UserId.Value);
        }

        if (filter.TransactionId.HasValue)
        {
            query = query.Where(t => t.Id == filter.TransactionId.Value);
        }

        if (filter.CreatedDateTo.HasValue)
        {
            query = query.Where(t => t.CreatedAt <= filter.CreatedDateTo.Value);
        }

        if (filter.CreatedDateFrom.HasValue)
        {
            query = query.Where(t => t.CreatedAt >= filter.CreatedDateFrom.Value);
        }

        if (filter.States?.Length > 0)
        {
            var states = filter.States.Select(Enum.Parse<TransactionState>);

            query = query.Where(t => states.Contains(t.State));
        }

        return query;
    }
}