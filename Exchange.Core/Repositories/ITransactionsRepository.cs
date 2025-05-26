using Exchange.Core.Pagination;
using Exchange.Domain.Entities;

namespace Exchange.Core.Repositories;

public interface ITransactionsRepository
{
    Task AddAsync(
        TransactionEntity transaction, 
        CancellationToken cancellationToken = default);
    
    void Update(TransactionEntity transaction);
    
    void DeleteRange(IEnumerable<TransactionEntity> transactions);
    
    Task<TransactionEntity?> GetByIdAsync(
        Guid id, 
        CancellationToken cancellationToken = default);
    
    Task<PagedResult<TransactionEntity>> GetAllAsync(
        TransactionsFilter filter, 
        CancellationToken cancellationToken = default);
    
    
}

public class TransactionsFilter
{
    public int? Count { get; set; }
    public int? Offset { get; set; }
    public Guid? UserId { get; set; }
    public Guid? TransactionId { get; set; }
    public bool IncludeUsers { get; set; }
    public string[]? States { get; set; }
    public DateTimeOffset? CreatedDateFrom { get; set; }
    public DateTimeOffset? CreatedDateTo{ get; set; }
}