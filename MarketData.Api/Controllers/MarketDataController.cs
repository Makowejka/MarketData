using MarketData.Domain.Contract;
using MarketData.Domain.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MarketData.Api.Controllers;

[Route("api/[controller]")]
public class MarketDataController : Controller
{
    private readonly IMarketDataService _marketDataService;

    public MarketDataController(IMarketDataService marketDataService)
    {
        _marketDataService = marketDataService;
    }

    [HttpGet]
    public async Task<List<MarketDataDto>> Get(GetMarketDataDto dto, CancellationToken ct)
    {
        var marketDataDto = await _marketDataService.Get(dto, ct);

        return marketDataDto;
    }
}
