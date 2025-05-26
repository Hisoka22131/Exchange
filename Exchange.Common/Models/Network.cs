using Exchange.Common.Enums;

namespace Exchange.Common.Models;

public class Network
{
    public required NetworkCode Code { get; init; }
    public required string Name { get; init; }
    public required string? WalletAddress { get; init; }
}