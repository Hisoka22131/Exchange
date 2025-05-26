namespace Exchange.Database.Options;

internal sealed record ExchangeDatabaseOptions
{
    public const string SectionName = "Database";
    
    public required string ConnectionString { get; init; }
    public required bool AutoMigrations { get; init; }
}