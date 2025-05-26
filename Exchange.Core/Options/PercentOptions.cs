namespace Exchange.Core.Options;

public record PercentOptions
{
    public const string SectionName = "Percents";
    
    public required Money Rub { get; init; }

    public record Money
    {
        public required decimal Buy { get; init; }
        public required decimal Sell { get; init; }
    }
}