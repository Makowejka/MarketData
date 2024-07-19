namespace MarketData.Domain.Contract;

public interface IMarketDataFilesRepository
{
    bool Exists(string filePath);
    Task<bool> IsUploadedAsync(string filePath, CancellationToken ct);
}
