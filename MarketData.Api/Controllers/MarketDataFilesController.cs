using MarketData.Domain.Contract;
using MarketData.Domain.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MarketData.Api.Controllers;

[Route("api/[controller]")]

public class MarketDataFilesController : Controller
{
    private readonly IMarketDataFileService _marketDataFileService;

    public MarketDataFilesController(IMarketDataFileService marketDataFileService)
    {
        _marketDataFileService = marketDataFileService;
    }

    // GET
    [HttpGet]
    public async Task<List<MarketDataFileDto>>Get(CancellationToken ct)
    {
        var marketDataFilesDto = await _marketDataFileService.Get(ct);

        return marketDataFilesDto;
    }
}
