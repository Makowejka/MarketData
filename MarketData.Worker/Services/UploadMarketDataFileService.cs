using MarketData.Domain.Messages;
using MarketData.Worker.Contract;

namespace MarketData.Worker.Services;

public class UploadMarketDataFileService: IUploadMarketDataFileService
{
    public async Task UploadAsync(UploadMarketDataFileMessage message, CancellationToken ct)
    {
        Console.WriteLine("Uploading file path: {0}", message.FilePath);
        await Task.Delay(2000, ct);
        Console.WriteLine("Uploaded file path: {0}", message.FilePath);
    }
}
