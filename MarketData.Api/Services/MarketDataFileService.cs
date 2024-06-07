using MarketData.Domain.Contract;
using MarketData.Domain.Dto;
using MarketData.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MarketData.Api.Services;

public class MarketDataFileService : IMarketDataFileService
{
    private readonly DataContext _context;

    public MarketDataFileService(DataContext context) => _context = context;

    public Task<List<MarketDataFileDto>> Get(CancellationToken ct)
    {
        return _context
            .MarketDataFile
            .Select(x =>
                new MarketDataFileDto(x.StartedAtUtc, x.CompletedAtUtc, x.FilePath, x.Status, x.ErrorMessage))
            .ToListAsync(cancellationToken: ct);
    }
}
