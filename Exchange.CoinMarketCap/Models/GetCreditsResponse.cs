using System.Text.Json.Serialization;
using Exchange.CoinMarketCap.Models.Base;

namespace Exchange.CoinMarketCap.Models;

public class GetCreditsResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public Data Data { get; set; } = new Data();
}

public class Data
{
    [JsonPropertyName("plan")]
    public Plan Plan { get; set; } = new Plan();

    [JsonPropertyName("usage")]
    public Usage Usage { get; set; } = new Usage();
}

public class Plan
{
    [JsonPropertyName("credit_limit_monthly")]
    public int CreditLimitMonthly { get; set; }

    [JsonPropertyName("credit_limit_monthly_reset")]
    public string CreditLimitMonthlyReset { get; set; } = string.Empty;

    [JsonPropertyName("credit_limit_monthly_reset_timestamp")]
    public DateTime CreditLimitMonthlyResetTimestamp { get; set; }

    [JsonPropertyName("rate_limit_minute")]
    public int RateLimitMinute { get; set; }
}

public class Usage
{
    [JsonPropertyName("current_minute")]
    public CurrentMinute CurrentMinute { get; set; } = new CurrentMinute();

    [JsonPropertyName("current_day")]
    public CurrentDay CurrentDay { get; set; } = new CurrentDay();

    [JsonPropertyName("current_month")]
    public CurrentMonth CurrentMonth { get; set; } = new CurrentMonth();
}

public class CurrentMinute
{
    [JsonPropertyName("requests_made")]
    public int RequestsMade { get; set; }

    [JsonPropertyName("requests_left")]
    public int RequestsLeft { get; set; }
}

public class CurrentDay
{
    [JsonPropertyName("credits_used")]
    public int CreditsUsed { get; set; }

    [JsonPropertyName("credits_left")]
    public int CreditsLeft { get; set; }
}

public class CurrentMonth
{
    [JsonPropertyName("credits_used")]
    public int CreditsUsed { get; set; }

    [JsonPropertyName("credits_left")]
    public int CreditsLeft { get; set; }
}
