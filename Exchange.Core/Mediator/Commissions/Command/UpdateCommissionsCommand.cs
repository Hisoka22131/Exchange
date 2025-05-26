using Exchange.Domain.Entities;
using MediatR;

namespace Exchange.Core.Mediator.Commissions.Command;

public record UpdateCommissionsCommand(IEnumerable<CommissionInfoEntity> CommissionInfoEntities) :  IRequest;