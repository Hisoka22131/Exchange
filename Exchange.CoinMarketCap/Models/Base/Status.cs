using System.Text.Json.Serialization;

namespace Exchange.CoinMarketCap.Models.Base;

public record Status
{
    [JsonPropertyName("timestamp")] public DateTimeOffset? Timestamp { get; init; }
    [JsonPropertyName("error_code")] public int? ErrorCode { get; init; }
    [JsonPropertyName("error_message")] public string? ErrorMessage { get; init; }
    [JsonPropertyName("elapsed")] public int? Elapsed { get; init; }
    [JsonPropertyName("credit_count")] public int? CreditCount { get; init; }
}