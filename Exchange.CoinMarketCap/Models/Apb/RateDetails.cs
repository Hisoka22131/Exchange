using System.Text.Json.Serialization;

namespace Exchange.CoinMarketCap.Models.Apb;

public class RateDetails
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("cc")]
    public string CurrencyCode { get; set; }

    [JsonPropertyName("descr")]
    public string Description { get; set; }

    [JsonPropertyName("value")]
    public string? Value { get; set; }

    [JsonPropertyName("value_buy")]
    public string? ValueBuy { get; set; }

    [JsonPropertyName("value_sell")]
    public string? ValueSell { get; set; }
}