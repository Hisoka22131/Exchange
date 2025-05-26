using System.Text.Json.Serialization;

namespace Exchange.CoinMarketCap.Models.Apb;

public class ExchangeData
{
    [JsonPropertyName("IB")]
    public ExchangeRates? Ib { get; set; }
}