using Exchange.Core.Pagination;
using Exchange.Domain.Entities;
using MediatR;

namespace Exchange.Core.Mediator.Users.Query.GetLastUserTransactions;

public record GetLastUserTransactionsQuery(string TelegramUserName, int? Count = 5) : IRequest<PagedResult<TransactionEntity>>;