namespace MarketData.Domain.Messages;

public record UploadMarketDataFileMessage
{
    public string? FilePath { get; set; }

    public UploadMarketDataFileMessage(string filePath)
    {
        FilePath = filePath;
    }
}
