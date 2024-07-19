using MarketData.Api.Options;
using MarketData.Domain.Contract;
using MarketData.Domain.Dto;
using MarketData.Domain.Messages;
using MarketData.Infrastructure.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MarketData.Api.Services;

public class MarketDataFileService : IMarketDataFileService
{
    private readonly DataContext _context;
    private readonly IBus _bus;
    private readonly IOptions<UploadMarketDataFileOptions> _options;

    public MarketDataFileService(DataContext context, IBus bus, IOptions<UploadMarketDataFileOptions> options)
    {
        _context = context;
        _bus = bus;
        _options = options;
    }

    public Task<List<MarketDataFileDto>> GetAsync(CancellationToken ct)
    {
        return _context
            .MarketDataFile
            .Select(x =>
                new MarketDataFileDto(x.StartedAtUtc, x.CompletedAtUtc, x.FilePath, x.Status, x.ErrorMessage))
            .ToListAsync(cancellationToken: ct);
    }

    public async Task UploadAsync(CancellationToken ct)
    {
        var folderPath = _options.Value.FolderPath ??
                         throw new InvalidOperationException(
                             $"{nameof(UploadMarketDataFileOptions.FolderPath)} cannot be null!");

        var filePaths = Directory.GetFiles(folderPath, "*.csv", SearchOption.TopDirectoryOnly);

        foreach (var filePath in filePaths)
        {
            var message = new UploadMarketDataFileMessage(filePath)
            {
                FilePath = filePath
            };

            await _bus.Publish(message, ct);
        }
    }
}
