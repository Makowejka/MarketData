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
    [HttpGet("get-uploaded")]
    public async Task<List<MarketDataFileDto>>GetAsync(CancellationToken ct)
    {
        return await _marketDataFileService.GetAsync(ct);
    }

    //POST
    [HttpPost("upload")]
    public async Task UploadAsync(CancellationToken ct)
    {
        await _marketDataFileService.UploadAsync(ct);
    }
}
