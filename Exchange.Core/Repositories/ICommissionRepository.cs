using Exchange.Domain.Entities;

namespace Exchange.Core.Repositories;

public interface ICommissionRepository
{
    Task<CommissionInfoEntity?> GetAsync(decimal amount, CancellationToken cancellationToken);
    Task<IList<CommissionInfoEntity>?> GetAllAsync(CancellationToken cancellationToken);
    
    void UpdateRangeAsync(IEnumerable<CommissionInfoEntity> entities);
}