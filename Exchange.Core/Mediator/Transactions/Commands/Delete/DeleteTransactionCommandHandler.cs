using Exchange.Core.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Core.Mediator.Transactions.Commands.Delete;

internal sealed class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand>
{
    private readonly IServiceProvider _serviceProvider;

    public DeleteTransactionCommandHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var filter = new TransactionsFilter
        {
            States = null,
            CreatedDateFrom = null,
            CreatedDateTo = null,
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
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}