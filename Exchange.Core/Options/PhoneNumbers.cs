namespace Exchange.Core.Options;

public record PhoneNumbers
{
    public const string SectionName = "PhoneNumbers";
    
    public required IDictionary<string, string> Values { get; init; }
}