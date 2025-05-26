using MediatR;

namespace Exchange.Core.Mediator.Assets.Query.GetAll;

public record GetAllAssetsQuery : IRequest<GetAllAssetsQueryResponse>;