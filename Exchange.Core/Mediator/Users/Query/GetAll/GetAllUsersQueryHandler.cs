using Exchange.Core.Pagination;
using Exchange.Core.Repositories;
using Exchange.Domain.Entities;
using MediatR;

namespace Exchange.Core.Mediator.Users.Query.GetAll;

internal sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PagedResult<UserEntity>>
{
    private readonly IUsersRepository _usersRepository;

    public GetAllUsersQueryHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<PagedResult<UserEntity>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var filter = new UsersFilter
        {
            Count = request.Count,
            Offset = request.Offset,
            TelegramUserName = request.TelegramUserName,
            IncludeTransactions = request.IncludeTransactions,
            TelegramUserId = request.TelegramUserId,
            UserId = request.UserId
        };

        return await _usersRepository.GetAllAsync(filter, cancellationToken);
    }
}