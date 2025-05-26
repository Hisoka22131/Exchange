namespace Exchange.CoinMarketCap.Options;

public class CoinMarketCapOptions
{
    public const string SectionName = "CoinMarketCap";
    public const string HttpClientName = "CoinMarketCap";
    
    public required Uri Url { get; init; }
    public required TimeSpan Timeout { get; init; }
    public required string ApiKey { get; init; }
    public required TimeSpan DelayBetweenReceivingCurrency { get; init; }
    public required TimeSpan DelayBetweenReceivingCredits { get; init; }
}