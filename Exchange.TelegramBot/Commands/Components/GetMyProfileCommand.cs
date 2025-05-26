using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Exchange.Common.Enums;
using Exchange.Core.Mediator.Users.Query.GetLastUserTransactions;
using Exchange.Domain.Enums;
using Exchange.TelegramBot.Models;
using MediatR;
using Telegram.Bot;

namespace Exchange.TelegramBot.Commands.Components;

internal sealed class GetMyProfileCommand : ITelegramCommand
{
    private readonly IMediator _mediator;
    private readonly ITelegramBotClient _telegramBotClient;


    public GetMyProfileCommand(IMediator mediator, ITelegramBotClient telegramBotClient)
    {
        _mediator = mediator;
        _telegramBotClient = telegramBotClient;
    }

    public bool CanProcess(string command)
    {
        return command.StartsWith(CommandsConstants.User.GetMyProfile);
    }

    public async Task ProcessingAsync(TelegramUser user, CancellationToken ct = default)
    {
        var query = new GetLastUserTransactionsQuery(
            TelegramUserName: user.Username,
            Count: null);

        var transactionsResult = await _mediator.Send(query, ct);

        var transactions = transactionsResult.Items
            .Where(x => x.State is TransactionState.Confirmed)
            .ToList();

        var statisticMessage = new StringBuilder();

        statisticMessage.AppendLine($"*Профиль* @{EscapeMarkdownV2(user.Username)}");
        statisticMessage.AppendLine();
        statisticMessage.AppendLine("\ud83d\udcca*Статистика*");

        statisticMessage.AppendLine($"\ud83d\udfe2Всего успешных сделок: {transactions.Count} шт.");
        statisticMessage.AppendLine($"\ud83d\udcc8Сделок на покупку: {transactions.Count(x => x.CurrencyTo.IsCrypto())}");
        statisticMessage.AppendLine($"\ud83d\udcc9Сделок на продажу: {transactions.Count(x => x.CurrencyTo.IsFiat())}");
        statisticMessage.AppendLine($"\ud83d\udcb8 Общая сумма сделок: {transactions.Sum(x => x.AmountToInUsdt)}");
        
        var currentMountTransactions = transactions
            .Where(x => x.CreatedAt.Month == DateTime.UtcNow.Month)
            .ToList();
        
        statisticMessage.AppendLine();
        statisticMessage.AppendLine("\ud83d\udcca*Статистика за текущий месяц*");

        statisticMessage.AppendLine($"\ud83d\udfe2Всего успешных сделок: {currentMountTransactions.Count} шт.");
        statisticMessage.AppendLine($"\ud83d\udcc8Сделок на покупку: {currentMountTransactions.Count(x => x.CurrencyTo.IsCrypto())}");
        statisticMessage.AppendLine($"\ud83d\udcc9Сделок на продажу: {currentMountTransactions.Count(x => x.CurrencyTo.IsFiat())}");
        statisticMessage.AppendLine($"\ud83d\udcb8 Общая сумма сделок: {currentMountTransactions.Sum(x => x.AmountToInUsdt)}");

        var userPhotos = await _telegramBotClient.GetUserProfilePhotos(user.ChatId, cancellationToken: ct);
        
        if (userPhotos.TotalCount == 0)
        {
            await _telegramBotClient.SendMessage(
                chatId: user.ChatId,
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                text: statisticMessage.ToString(),
                cancellationToken: ct);
        }
        else
        {
            await _telegramBotClient.SendPhoto(
                chatId: user.ChatId,
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                photo: userPhotos.Photos.First().Last().FileId,
                caption: statisticMessage.ToString(),
                cancellationToken: ct);
        }
    }

    private static string EscapeMarkdownV2(string text)
    {
        return Regex.Replace(text, @"(?<!\\)([_*\[\]()~`>#+\-=|{}\.!])", @"\$1");
    }
}