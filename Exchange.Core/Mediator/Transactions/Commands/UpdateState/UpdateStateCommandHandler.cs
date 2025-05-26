using Exchange.Core.Repositories;
using Exchange.Domain.Enums;
using MediatR;

namespace Exchange.Core.Mediator.Transactions.Commands.UpdateState;

internal sealed class UpdateStateCommandHandler : IRequestHandler<UpdateStateCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateStateCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateStateCommand request, CancellationToken cancellationToken)
    {
        if (request.State is TransactionState.Unknown)
        {
            throw new ArgumentException("Неизвестное состояние транзакции", nameof(request.State));
        }
        
        var transaction = await _unitOfWork.Transactions.GetByIdAsync(request.Id, cancellationToken);

        if (transaction is null)
        {
            throw new ArgumentNullException(nameof(transaction), "Транзакция не найдена");
        }

        if (transaction.State is TransactionState.Confirmed or TransactionState.Rejected)
        {
            throw new Exception("Транзакция уже завершена, её нельзя изменить!");
        }
        
        transaction.State = request.State;
        
        _unitOfWork.Transactions.Update(transaction);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}