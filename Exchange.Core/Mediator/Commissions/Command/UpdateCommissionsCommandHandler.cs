using Exchange.Core.Repositories;
using MediatR;

namespace Exchange.Core.Mediator.Commissions.Command;

internal sealed class UpdateCommissionsCommandHandler : IRequestHandler<UpdateCommissionsCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCommissionsCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateCommissionsCommand request, CancellationToken cancellationToken)
    {
        if (request.CommissionInfoEntities.Any(x => x.PercentFee is < 0 or > 1))
        {
            throw new ArgumentException("Процентная комиссия должна быть в диапазоне от 0 до 1");
        }
        
        _unitOfWork.Commissions.UpdateRangeAsync(request.CommissionInfoEntities);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}