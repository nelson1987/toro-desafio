using Microsoft.AspNetCore.Mvc;
using Toro.Core.Entities;

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
public record CreateOrderCommand
{
    public string Symbol { get; set; }
    public int Amount { get; set; }
}

public record TrendQueryResponse
{
    public string Symbol { get; set; }
    public decimal CurrentPrice { get; set; }
}
public interface ITrendRepository
{
    Task<List<Trend>> BestTrend(int listSize = 5);
    Task<Trend?> GetBySymbol(string symbol);
}
public class TrendRepository : ITrendRepository
{
    private readonly List<Trend> trends = new List<Trend>()
    {
        TrendBuilder.Create("PETR4", 28.44M, 10),
        TrendBuilder.Create("MGLU3", 25.91M, 9),
        TrendBuilder.Create("VVAR3", 25.91M, 8),
        TrendBuilder.Create("SANB11", 40.77M, 7),
        TrendBuilder.Create("TORO4", 115.98M, 6),
    };
    public async Task<List<Trend>> BestTrend(int listSize = 5)
    {
        return trends.OrderByDescending(x => x.Buys).Take(listSize).ToList();
    }
    public async Task<Trend?> GetBySymbol(string symbol)
    {
        return trends.FirstOrDefault(x => x.Symbol == symbol);
    }
}
public static class TrendBuilder
{
    public static Trend Create(string symbol, decimal price, int buys)
    {
        return new Trend()
        {
            Symbol = symbol,
            Buys = buys,
            CurrentPrice = price
        };
    }
}