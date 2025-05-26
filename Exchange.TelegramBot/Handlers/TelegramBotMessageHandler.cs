using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Exchange.Domain.Interfaces;
using Exchange.TelegramBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Exchange.TelegramBot.Handlers;

[SuppressMessage("ReSharper", "DuplicatedSwitchSectionBodies")]
internal sealed class TelegramBotMessageHandler : IMessageHandler
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IEnumerable<ITelegramMessagesHandler> _messagesHandlers;

    public TelegramBotMessageHandler(
        ITelegramBotClient telegramBotClient,
        IEnumerable<ITelegramMessagesHandler> messagesHandlers
    )
    {
        _telegramBotClient = telegramBotClient;
        _messagesHandlers = messagesHandlers;
    }

    public async Task HandleMessageAsync(string message, CancellationToken ct = default)
    {
        if (!TryParse(message, out Update update))
        {
            return;
        }

        try
        {
            var handler = _messagesHandlers.FirstOrDefault(x => x.CanProcess(update.Type));
            
            if (handler is null)
            {
                await _telegramBotClient.SendMessage(chatId:GetChatId(update), text: "Неизвестный тип сообщения.", cancellationToken: ct);
                return;
            }
            
            await handler.HandleMessageAsync(update, ct);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            
            await _telegramBotClient.SendMessage(
                GetChatId(update),
                ex.Message,
                cancellationToken: ct);
        }
    }

    private static bool TryParse(string text, out Update message)
    {
        message = null!;

        try
        {
            message = JsonSerializer.Deserialize<Update>(text, JsonSerializerOptions)!;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Could not parse the message {text}.");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }

        return true;
    }

    private static long GetChatId(Update update)
    {
        return update.Message?.Chat.Id
               ?? update.CallbackQuery?.From.Id
               ?? update.EditedMessage?.From?.Id
               ?? update.MyChatMember?.From?.Id
               ?? throw new InvalidOperationException("Unknown chat");
    }
    
    private static int GetMessageId(Update update)
    {
        return update.Message?.Id
               ?? update.CallbackQuery?.Message?.Id
               ?? update.EditedMessage?.MessageId
               ?? throw new InvalidOperationException("Unknown message");
    }
}