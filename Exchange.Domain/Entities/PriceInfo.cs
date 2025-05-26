namespace Exchange.Domain.Entities;

public class PriceInfo
{
    public required string Name { get; set; }
    public required decimal Price { get; set; }
    public required bool IsGrow { get; set; }
}