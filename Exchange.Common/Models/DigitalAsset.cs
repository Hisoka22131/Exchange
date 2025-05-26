using Exchange.Common.Enums;

namespace Exchange.Common.Models;

public class DigitalAsset
{
    public required Currency Value { get; init; }
    public required string Icon { get; init; }
    public required string Label { get; init; }
    public required List<Network> Networks { get; init; }
}