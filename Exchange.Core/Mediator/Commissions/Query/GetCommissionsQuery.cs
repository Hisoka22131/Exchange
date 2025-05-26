using Exchange.Domain.Entities;
using MediatR;

namespace Exchange.Core.Mediator.Commissions.Query;

public record GetCommissionsQuery : IRequest<IEnumerable<CommissionInfoEntity>?>;