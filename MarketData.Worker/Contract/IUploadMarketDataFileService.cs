using MarketData.Domain.Messages;

namespace MarketData.Worker.Contract;

public interface IUploadMarketDataFileService
{
    Task UploadAsync(UploadMarketDataFileMessage message, CancellationToken ct);
}
