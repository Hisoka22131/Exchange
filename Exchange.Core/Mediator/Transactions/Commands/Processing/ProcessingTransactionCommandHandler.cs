using Exchange.Core.Records;
using Exchange.Core.Repositories;
using Exchange.Domain.Enums;
using Exchange.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Core.Mediator.Transactions.Commands.Processing;

internal sealed class ProcessingTransactionCommandHandler : IRequestHandler<ProcessingTransactionCommand>
{
    private readonly IServiceProvider _serviceProvider;

    public ProcessingTransactionCommandHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Handle(
        ProcessingTransactionCommand req,
        CancellationToken cancellationToken
    )
    {
        using var scope = _serviceProvider.CreateScope();

        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var adminMessageSender = scope.ServiceProvider.GetRequiredService<ITelegramAdminMessageSender>();

        var transactionEntity = await unitOfWork.Transactions.GetByIdAsync(
            req.TransactionId, 
            cancellationToken);

        if (transactionEntity is null)
        {
            throw new ArgumentNullException(nameof(transactionEntity), "Transaction not found");
        }

        transactionEntity.State = TransactionState.Processing;
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var message = new TransactionMessage(transactionEntity);

        await adminMessageSender.SendMessageAsync(
            message: message.ToString(),
            transactionId: transactionEntity.Id,
            useCallbackData: true,
            cancellationToken: cancellationToken);
    }
}