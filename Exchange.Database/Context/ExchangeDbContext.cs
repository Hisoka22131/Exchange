using System.Reflection;
using Exchange.Domain.Entities;
using Exchange.Domain.Entities.Interfaces;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;

namespace Exchange.Database.Context;

public sealed class ExchangeDbContext : DbContext, IDataProtectionKeyContext
{
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<TransactionEntity> Transactions => Set<TransactionEntity>();
    public DbSet<CommissionInfoEntity> Commissions => Set<CommissionInfoEntity>();
    public DbSet<DataProtectionKey> DataProtectionKeys => Set<DataProtectionKey>();
    
    public ExchangeDbContext(DbContextOptions<ExchangeDbContext> options)
        : base(options)
    {
        SavingChanges += OnSavingChanges;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void OnSavingChanges(object? sender, SavingChangesEventArgs savingChangesEventArgs)
    {
        SetAuditProperties();
    }
    
    private void SetAuditProperties()
    {
        var now = DateTimeOffset.Now;

        var createdEntities = ChangeTracker.Entries()
            .Where(x => x.State is EntityState.Added)
            .Select(x => x.Entity)
            .OfType<ICreatable>();
        
        foreach (var entity in createdEntities)
        {
            entity.CreatedAt = now;
        }
        
        var updatedEntities = ChangeTracker.Entries()
            .Where(x => x.State is EntityState.Added or EntityState.Modified)
            .Select(x => x.Entity)
            .OfType<IUpdatable>();
        
        foreach (var entity in updatedEntities)
        {
            entity.UpdatedAt = now;
        }
    }
}