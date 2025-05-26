namespace Exchange.Core.Extensions;

public static class DecimalExtensions
{
    public static decimal RoundToDecimalPlaces(this decimal value, int decimalPlaces)
    {
        return Math.Round(value, decimalPlaces, MidpointRounding.AwayFromZero);
    }
    
    public static decimal RoundToDecimalSevenPlaces(this decimal value)
    {
        return value.RoundToDecimalPlaces(7);
    }
    
    /// <summary>
    /// Округление в большую сторону (до 6 знаков)
    /// </summary>
    public static decimal ToCeilingValue(this decimal value)
    {
        return Math.Ceiling(value * 100_0000M) / 100_0000M;
    }
}