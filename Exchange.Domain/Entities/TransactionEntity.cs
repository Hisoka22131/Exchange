using Exchange.Common.Enums;
using Exchange.Domain.Entities.Interfaces;
using Exchange.Domain.Enums;

namespace Exchange.Domain.Entities;

public class TransactionEntity : ICreatable, IUpdatable
{
    public required Guid Id { get; set; }
    public required Currency CurrencyFrom { get; set; }
    public required Currency CurrencyTo { get; set; }
    public required decimal AmountFrom { get; set; }
    public required decimal AmountTo { get; set; }
    public required decimal Commission { get; set; }
    public required string City { get; set; }
    public required string PhoneNumberUser { get; set; }
    public required NetworkCode CryptoNetworkCode { get; set; }
    public required string CryptoNetworkName { get; set; }
    public required NetworkCode FiatNetworkCode { get; set; }
    public required string FiatNetworkName { get; set; }
    public required string WalletAddressUser { get; set; }
    public required string WalletAddressAdmin { get; set; }
    public required TransactionState State { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public required UserEntity User { get; set; }
    public required Guid UserId { get; set; }
    public required decimal AmountToInUsdt { get; set; }
}