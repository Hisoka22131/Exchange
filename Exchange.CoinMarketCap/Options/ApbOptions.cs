namespace Exchange.CoinMarketCap.Options;

public class ApbOptions
{
    public const string SectionName = "AgroPromBank";
    public const string HttpClientName = "AgroPromBank";
    
    public required Uri Url { get; init; }
    public required TimeSpan Timeout { get; init; }
}