using Exchange.Common.Extensions;
using Exchange.Core.Repositories;
using Exchange.Domain.Enums;
using Exchange.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Core.Mediator.Transactions.Commands.Complete;

internal sealed class CompleteTransactionCommandHandler : IRequestHandler<CompleteTransactionCommand>
{
    private readonly IServiceProvider _serviceProvider;

    public CompleteTransactionCommandHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Handle(
        CompleteTransactionCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            if (request.TransactionState != TransactionState.Confirmed &&
                request.TransactionState != TransactionState.Rejected)
            {
                throw new ArgumentException("Неверное состояние транзакции");
            }

            using var scope = _serviceProvider.CreateScope();

            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var telegramUserMessageSender = scope.ServiceProvider.GetRequiredService<ITelegramUserMessageSender>();

            var transaction = await unitOfWork.Transactions.GetByIdAsync(request.TransactionId, cancellationToken);

            if (transaction is null)
            {
                throw new ArgumentNullException(nameof(transaction), "Транзакция не найдена");
            }

            if (transaction.State != TransactionState.Processing)
            {
                throw new ArgumentException("Транзакция уже завершена");
            }

            transaction.State = request.TransactionState;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var symbol = transaction.State == TransactionState.Confirmed ? "✅" : "❌";

            var message = $"Ваша транзакция с идентификатором '`{transaction.Id}`' успешно обработана.\n" +
                          $"Сумма отправления: {transaction.AmountFrom:G29} {transaction.CurrencyFrom}\n" +
                          $"Сумма получения: {transaction.AmountTo:G29} {transaction.CurrencyTo}\n" +
                          $"Текущий статус: {symbol} {transaction.State.GetDescription()}.\n" +
                          "Спасибо за использование нашего сервиса!";
            
            await telegramUserMessageSender.SendMessageAsync(message,
                transaction.User.TelegramUserId,
                cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            var adminChantSender = _serviceProvider.GetRequiredService<ITelegramAdminMessageSender>();
            
            await adminChantSender.SendMessageAsync(
                $"Произошла ошибка при обработке транзакции с Id = `{request.TransactionId}`\n"
                + $"*{ex.Message}!*",
                request.TransactionId,
                false,
                cancellationToken);
        }
    }
}