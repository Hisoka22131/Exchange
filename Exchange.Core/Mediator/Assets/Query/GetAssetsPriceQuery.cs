using Exchange.Domain.Entities;
using MediatR;

namespace Exchange.Core.Mediator.Assets.Query;

public record GetAssetsPriceQuery : IRequest<PriceInfo[]>;