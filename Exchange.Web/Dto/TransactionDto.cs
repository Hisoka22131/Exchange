using Exchange.Common.Enums;
using Exchange.Domain.Entities;
using Exchange.Domain.Enums;

namespace Exchange.Web.Dto;

public class TransactionDto
{
    public Guid Id { get; set; }
    public Currency CurrencyFrom { get; set; }
    public Currency CurrencyTo { get; set; }
    public decimal AmountFrom { get; set; }
    public decimal AmountTo { get; set; }
    public decimal Commission { get; set; }
    public string City { get; set; }
    public string PhoneNumberUser { get; set; }
    public NetworkCode CryptoNetworkCode { get; set; }
    public string CryptoNetworkName { get; set; }
    public NetworkCode FiatNetworkCode { get; set; }
    public string FiatNetworkName { get; set; }
    public string WalletAddressUser { get; set; }
    public string WalletAddressAdmin { get; set; }
    public TransactionState State { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public Guid UserId { get; set; }

    public TransactionDto(TransactionEntity entity)
    {
        Id = entity.Id;
        CurrencyFrom = entity.CurrencyFrom;
        CurrencyTo = entity.CurrencyTo;
        AmountFrom = entity.AmountFrom;
        AmountTo = entity.AmountTo;
        Commission = entity.Commission;
        City = entity.City;
        PhoneNumberUser = entity.PhoneNumberUser;
        CryptoNetworkCode = entity.CryptoNetworkCode;
        CryptoNetworkName = entity.CryptoNetworkName;
        FiatNetworkName = entity.FiatNetworkName;
        FiatNetworkCode = entity.FiatNetworkCode;
        WalletAddressUser = entity.WalletAddressUser;
        WalletAddressAdmin = entity.WalletAddressAdmin;
        State = entity.State;
        CreatedAt = entity.CreatedAt;
        UpdatedAt = entity.UpdatedAt;
        UserId = entity.UserId;
    }
}