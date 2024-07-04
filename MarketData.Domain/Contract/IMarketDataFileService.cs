using MarketData.Domain.Dto;

namespace MarketData.Domain.Contract;

public interface IMarketDataFileService
{
    Task<List<MarketDataFileDto>> GetAsync(CancellationToken ct);

    Task UploadAsync(CancellationToken ct);
}
