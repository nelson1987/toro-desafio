using Microsoft.AspNetCore.Mvc;
using Toro.Core.Features.Trends;

namespace Toro.Api.Controllers;

[ApiController]
[Route("[Controller]")]
public class TrendController : ControllerBase
{
    private readonly TrendRepository _trendRepository;
    public TrendController()
    {
        _trendRepository = new TrendRepository();
    }

    [HttpGet("/trends")]
    public async Task<IActionResult> Get(int size = 5)
    {
        var bestTrends = await _trendRepository.BestTrend(size);
        List<TrendQueryResponse> response = bestTrends.Select(x => new TrendQueryResponse()
        {
            CurrentPrice = x.CurrentPrice,
            Symbol = x.Symbol
        }).ToList();
        return StatusCode(200, response);
    }
}