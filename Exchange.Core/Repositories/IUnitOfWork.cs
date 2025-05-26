namespace Exchange.Core.Repositories;

public interface IUnitOfWork : IDisposable
{
    IUsersRepository Users { get; }
    ICommissionRepository Commissions { get; }
    ITransactionsRepository Transactions { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}