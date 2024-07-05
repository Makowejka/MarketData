namespace MarketData.Domain.Entities;

public class MarketData
{
    public DateTime TimeUtc { get; set; }

    public string Asset { get; set; } = null!;

    public decimal Price { get; set; }
}
