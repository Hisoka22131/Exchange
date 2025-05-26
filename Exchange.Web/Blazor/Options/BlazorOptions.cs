namespace Exchange.Web.Blazor.Options;

public record BlazorOptions
{
    public const string SectionName = "Blazor";
    public const string ClientName = "ExchangeApi";
    
    public required Uri Url { get; init; }
    public required TimeSpan Timeout { get; init; }
}
