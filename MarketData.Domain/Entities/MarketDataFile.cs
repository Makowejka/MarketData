using MarketData.Domain.Model;

namespace MarketData.Domain.Entities;

public class MarketDataFile
{
    public long Id { get; set; }

    public DateTime StartedAtUtc { get; set; }

    public DateTime? CompletedAtUtc { get; set; }

    public string FilePath { get; set; } = null!;

    public MarketDataFileUploadStatus Status { get; set; }

    public string? ErrorMessage { get; set; }
}
