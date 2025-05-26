using Exchange.Core.Repositories;
using Exchange.Domain.Entities;
using MediatR;

namespace Exchange.Core.Mediator.Commissions.Query;

internal sealed class GetCommissionsQueryHandler : IRequestHandler<GetCommissionsQuery , IEnumerable<CommissionInfoEntity>?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCommissionsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CommissionInfoEntity>?> Handle(GetCommissionsQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Commissions.GetAllAsync(cancellationToken);
    }
}