using Exchange.Common.Enums;
using Exchange.Domain.Enums;

namespace Exchange.Core.Records;

public record Customer(
    Currency Currency,
    string? Network,
    string City,
    string PhoneNumber
);