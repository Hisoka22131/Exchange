using Exchange.Core.Mediator.Transactions.Commands.Complete;
using Exchange.Domain.Enums;
using Exchange.TelegramBot.Extensions;
using Exchange.TelegramBot.Models;
using Exchange.TelegramBot.Options;
using MediatR;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace Exchange.TelegramBot.Commands.Components;

internal sealed class RejectTransactionCommand : ITelegramCommand
{
    private readonly IMediator _mediator;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly long _chatId;

    public RejectTransactionCommand(IMediator mediator, ITelegramBotClient telegramBotClient, IOptions<TelegramBotOptions> options)
    {
        _mediator = mediator;
        _telegramBotClient = telegramBotClient;
        _chatId = options.Value.AdminChatId;
    }

    public bool CanProcess(string command)
    {
        return command.StartsWith(CommandsConstants.Transactions.Reject);
    }

    public async Task ProcessingAsync(TelegramUser user, CancellationToken ct = default)
    {
        var tuple = TelegramCommandParser.ParseCommand(user.Command);
        
        var transactionId = tuple?.TransactionId;
        
        if (transactionId is null)
        {
            throw new ArgumentNullException(nameof(tuple), "Транзакция не найдена");
        }

        var command = new CompleteTransactionCommand(
            transactionId.Value,
            TransactionState.Rejected);
        
        await _mediator.Send(command, ct);

        if (user.MessageId.HasValue)
        {
            await _telegramBotClient.DeleteMessage(_chatId, user.MessageId.Value, ct);
            await _telegramBotClient.SendMessage(_chatId, $"Транзакция с Id = {transactionId} отклонена.", cancellationToken: ct);
        }
    }
}