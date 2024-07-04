using MarketData.Domain.Messages;
using MarketData.Worker.Contract;
using MassTransit;

namespace MarketData.Worker.Consumers;

public class UploadMarketDataFileConsumer : IConsumer<UploadMarketDataFileMessage>
{
    private readonly IUploadMarketDataFileService _uploadMarketDataFileService;

    public UploadMarketDataFileConsumer(IUploadMarketDataFileService uploadMarketDataFileService)
    {
        _uploadMarketDataFileService = uploadMarketDataFileService;
    }

    public async Task Consume(ConsumeContext<UploadMarketDataFileMessage> context)
    {
        await _uploadMarketDataFileService.UploadAsync(context.Message, context.CancellationToken);
    }
}
