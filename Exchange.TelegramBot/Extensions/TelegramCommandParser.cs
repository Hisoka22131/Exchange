using System.Text.RegularExpressions;

namespace Exchange.TelegramBot.Extensions;

internal static class TelegramCommandParser
{
    private static readonly Regex CommandRegex = new(@"^/(confirm|reject):(?<id>[0-9a-fA-F\-]{36})$", RegexOptions.Compiled);

    public static (string Action, Guid? TransactionId)? ParseCommand(string command)
    {
        if (string.IsNullOrWhiteSpace(command))
            return null;

        var match = CommandRegex.Match(command);

        if (!match.Success)
            return null;

        var action = match.Groups[1].Value;
        Guid? transactionId = Guid.TryParse(match.Groups["id"].Value, out var guid) ? guid : null;

        return (action, transactionId);
    }
}