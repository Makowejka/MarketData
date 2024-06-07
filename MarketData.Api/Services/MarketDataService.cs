using MarketData.Domain.Contract;
using MarketData.Domain.Dto;
using MarketData.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MarketData.Api.Services;

public class MarketDataService : IMarketDataService
{
    private readonly DataContext _context;

    public MarketDataService(DataContext context) => _context = context;

    public Task<List<MarketDataDto>> Get(GetMarketDataDto dto, CancellationToken ct)
    {
        return _context
            .MarketData
            .Select(x => new MarketDataDto(x.Asset, x.TimeUtc, x.Price))
            .ToListAsync(ct);
    }
}
