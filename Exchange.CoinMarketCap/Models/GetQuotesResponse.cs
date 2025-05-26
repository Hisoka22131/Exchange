using System.Text.Json.Serialization;
using Exchange.CoinMarketCap.Models.Base;

namespace Exchange.CoinMarketCap.Models;

public class GetQuotesResponse : BaseResponse
{
    [JsonPropertyName("data")] public Dictionary<string, CurrencyData>? Data { get; set; }
}

public class CurrencyData
{
    [JsonPropertyName("name")] public string? Name { get; set; }

    [JsonPropertyName("symbol")] public string? Symbol { get; set; }

    [JsonPropertyName("quote")] public Dictionary<string, Quote> Quote { get; set; }
}

public class Quote
{
    [JsonPropertyName("price")] public decimal Price { get; set; }
    [JsonPropertyName("percent_change_1h")] public decimal PercentChange1H { get; set; }
}