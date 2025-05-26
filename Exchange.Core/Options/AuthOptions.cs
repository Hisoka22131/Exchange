namespace Exchange.Core.Options;

public record AuthOptions
{
    public const string SectionName = "Auth";
    
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required string SecretKey { get; init; }
}