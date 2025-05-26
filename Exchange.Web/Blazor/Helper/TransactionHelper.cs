using Exchange.Domain.Enums;
using MudBlazor;

namespace Exchange.Web.Blazor.Helper;

public static class TransactionHelper
{
    public static IEnumerable<TransactionState> GetValues()
    {
        return Enum.GetValues<TransactionState>()
            .Where(x => x != TransactionState.Unknown);
    }
    
    public static Color GetColor(TransactionState state) =>
        state switch
        {
            TransactionState.Processing => Color.Primary,
            TransactionState.Confirmed => Color.Success,
            TransactionState.Rejected => Color.Error,
            TransactionState.Init => Color.Info,
            _ => default
        };
}