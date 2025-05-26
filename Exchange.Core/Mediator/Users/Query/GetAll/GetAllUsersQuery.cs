using Exchange.Core.Pagination;
using Exchange.Domain.Entities;
using MediatR;

namespace Exchange.Core.Mediator.Users.Query.GetAll;

public record GetAllUsersQuery(
    int? Count,
    int? Offset,
    long? TelegramUserId,
    Guid? UserId,
    string? TelegramUserName,
    bool IncludeTransactions
) : IRequest<PagedResult<UserEntity>>;