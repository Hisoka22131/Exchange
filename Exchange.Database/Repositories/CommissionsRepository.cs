using Exchange.Core.Repositories;
using Exchange.Database.Context;
using Exchange.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Exchange.Database.Repositories;

internal sealed class CommissionsRepository : ICommissionRepository
{
    private readonly ExchangeDbContext _exchangeDbContext;

    public CommissionsRepository(ExchangeDbContext exchangeDbContext)
    {
        _exchangeDbContext = exchangeDbContext;
    }

    public async Task<CommissionInfoEntity?> GetAsync(decimal amount, CancellationToken cancellationToken)
    {
        return await _exchangeDbContext.Commissions
            .FirstOrDefaultAsync(
                predicate: c => c.AmountFrom <= amount && (c.AmountTo == null || c.AmountTo >= amount),
                cancellationToken: cancellationToken);
    }

    public async Task<IList<CommissionInfoEntity>?> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _exchangeDbContext.Commissions.ToListAsync(cancellationToken);
    }
    
    public void UpdateRangeAsync(IEnumerable<CommissionInfoEntity> entities)
    {
        _exchangeDbContext.Commissions.UpdateRange(entities);
    }
}