using FluentValidation;
using MarketData.Api.ExceptionHandler;
using MarketData.Domain.Contract;
using MarketData.Domain.Dto;
using MarketData.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MarketData.Api.Services;

public class MarketDataService : IMarketDataService
{
    private readonly DataContext _context;
    private readonly IValidator<GetMarketDataDto> _validator;

    public MarketDataService(DataContext context, IValidator<GetMarketDataDto> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<List<MarketDataDto>> Get(GetMarketDataDto dto, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(dto, ct);

        if (validationResult.IsValid == false)
        {
            throw new BadRequestException(validationResult.Errors);
        }

        return await _context
            .MarketData
            .Select(x => new MarketDataDto(x.Asset, x.TimeUtc, x.Price))
            .ToListAsync(ct);
    }
}
