using System.Text.Json.Serialization;

namespace Exchange.CoinMarketCap.Models.Base;

public class BaseResponse
{
    [JsonPropertyName("status")]
    public Status? Status { get; set; }
}