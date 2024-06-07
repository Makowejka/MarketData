using MarketData.Domain.Dto;

namespace MarketData.Domain.Contract;

public interface IMarketDataFileService
{
    Task<List<MarketDataFileDto>> Get(CancellationToken ct);

}
