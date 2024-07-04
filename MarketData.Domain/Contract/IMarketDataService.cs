using MarketData.Domain.Dto;

namespace MarketData.Domain.Contract;

public interface IMarketDataService
{
    Task<List<MarketDataDto>> GetAsync(GetMarketDataDto dto, CancellationToken ct);

}
