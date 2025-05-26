using System.Text.Json.Serialization;
using Exchange.Common.Enums;
using Exchange.Domain.Enums;

namespace Exchange.Web.Blazor.Services.Transactions.Dto;

public class TransactionBlazorDto
{
    public Guid Id { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Currency CurrencyFrom { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Currency CurrencyTo { get; set; }

    public decimal AmountFrom { get; set; }
    public decimal AmountTo { get; set; }
    public decimal Commission { get; set; }
    public string City { get; set; }
    public string PhoneNumberUser { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public NetworkCode CryptoNetworkCode { get; set; }
    public string CryptoNetworkName { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public NetworkCode FiatNetworkCode { get; set; }
    public string FiatNetworkName { get; set; }
    public string WalletAddressUser { get; set; }
    public string WalletAddressAdmin { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TransactionState State { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public Guid UserId { get; set; }
}