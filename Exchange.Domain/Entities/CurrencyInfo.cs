namespace Exchange.Domain.Entities;

public class CurrencyInfo
{
    public Dictionary<string, CurrencyData> Data { get; set; } = new();
}

public class CurrencyData
{
    public string? Name { get; set; }

    public string? Symbol { get; set; }

    public Dictionary<string, Quote> Quote { get; set; } = new();
}

public class Quote
{
    public decimal Price { get; set; }
    public bool IsGrow { get; set; }
}