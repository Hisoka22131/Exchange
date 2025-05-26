using Telegram.Bot.Types.ReplyMarkups;

namespace Exchange.TelegramBot.Extensions;

public static class InlineKeyboardMarkupExtensions
{
    public static InlineKeyboardMarkup CreateWithMenu(IEnumerable<IEnumerable<InlineKeyboardButton>> inlineKeyboard)
    {
        var keyboardList = inlineKeyboard
            .Select(row => new List<InlineKeyboardButton>(row))
            .ToList();

        keyboardList.Add([InlineKeyboardButton.WithCallbackData("🗂️ Меню", CommandsConstants.Common.Start)]);

        return new InlineKeyboardMarkup(keyboardList);
    }
    
    public static InlineKeyboardMarkup CreateWithMenu()
    {
        return new InlineKeyboardMarkup(
        [
            [InlineKeyboardButton.WithCallbackData("🗂️ Меню", CommandsConstants.Common.Start)]
        ]);
    }
}