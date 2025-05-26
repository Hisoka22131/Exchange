using Exchange.Core.Repositories;
using Exchange.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Exchange.Core.Hosted;

internal sealed class ClearTransactionsBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public ClearTransactionsBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            if (now is { Hour: 3, Minute: < 30 })
            {
                await ClearTransactionsAsync(stoppingToken);
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }

    private async Task ClearTransactionsAsync(CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var filter = new TransactionsFilter
        {
            States = [TransactionState.Init.ToString()],
            CreatedDateFrom = null,
            CreatedDateTo = DateTimeOffset.Now.AddMinutes(-10),
            Count = null,
            Offset = null,
            IncludeUsers = false,
            TransactionId = null,
            UserId = null
        };

        var transactionsForDelete = await unitOfWork.Transactions.GetAllAsync(filter, cancellationToken);
        
        if (!transactionsForDelete.Items.Any())
        {
            return;
        }
        
        unitOfWork.Transactions.DeleteRange(transactionsForDelete.Items);
        
        await unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);
    }
}