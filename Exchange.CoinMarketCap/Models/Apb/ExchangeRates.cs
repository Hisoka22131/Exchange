using System.Text.Json.Serialization;

namespace Exchange.CoinMarketCap.Models.Apb;

public class ExchangeRates
{
    [JsonPropertyName("rates")]
    public Dictionary<string, RateDetails?>? Rates { get; set; }

    [JsonPropertyName("date")]
    public string? Date { get; set; }

    [JsonPropertyName("found")]
    public int Found { get; set; }
}