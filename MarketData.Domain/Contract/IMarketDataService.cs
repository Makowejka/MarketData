using MarketData.Domain.Dto;

namespace MarketData.Domain.Contract;

public interface IMarketDataService
{
    Task<List<MarketDataDto>> Get(GetMarketDataDto dto, CancellationToken ct);

}
