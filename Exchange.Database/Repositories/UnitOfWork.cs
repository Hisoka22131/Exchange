using Exchange.Core.Repositories;
using Exchange.Database.Context;

namespace Exchange.Database.Repositories;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly ExchangeDbContext _exchangeDbContext;
    
    public UnitOfWork(ExchangeDbContext exchangeDbContext)
    {
        _exchangeDbContext = exchangeDbContext;
        Users = new UsersRepository(_exchangeDbContext);
        Transactions = new TransactionsRepository(_exchangeDbContext);
        Commissions = new CommissionsRepository(_exchangeDbContext);
    }

    public IUsersRepository Users { get; }
    public ICommissionRepository Commissions { get; }
    public ITransactionsRepository Transactions { get; }
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _exchangeDbContext.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _exchangeDbContext.Dispose();
    }
}