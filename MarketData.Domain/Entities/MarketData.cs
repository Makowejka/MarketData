namespace MarketData.Domain.Entities;

public class MarketData
{
    public long Id { get; set; }

    public string Asset { get; set; } = null!;

    public DateTime TimeUtc { get; set; }

    public decimal Price { get; set; }
}
