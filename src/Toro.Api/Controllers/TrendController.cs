using Microsoft.AspNetCore.Mvc;
using Toro.Core.Entities;
using Toro.Core.Repositories;

namespace Toro.Api.Controllers;

[ApiController]
[Route("[Controller]")]
public class TrendController : ControllerBase
{
    private readonly TrendRepository _trendRepository;
    private readonly AccountRepository _accountRepository;
    public TrendController()
    {
        _trendRepository = new TrendRepository();
        _accountRepository = new AccountRepository();
    }

    [HttpGet("/trends")]
    public async Task<IActionResult> Trends()
    {
        var bestTrends = _trendRepository.BestTrend(3);
        List<TrendQueryResponse> response = bestTrends.Select(x => new TrendQueryResponse()
        {
            CurrentPrice = x.CurrentPrice,
            Symbol = x.Symbol
        }).ToList();
        return StatusCode(200, response);
    }
    [HttpPost("/orders")]
    public async Task<IActionResult> Order(CreateOrderCommand command)
    {
        var trend = await _trendRepository.GetBySymbol(command.Symbol);
        if (trend == null) { return NotFound(); }

        var user = await _accountRepository.GetAccountBy(x => x.Account == "300123");
        if (user == null) { return NotFound(); }

        user.BuyOrder(trend!, command.Amount);

        return StatusCode(201);
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
public class TrendRepository
{
    private readonly List<Trend> trends = new List<Trend>()
    {
        TrendBuilder.Create("PETR4", 28.44M, 10),
        TrendBuilder.Create("MGLU3", 25.91M, 9),
        TrendBuilder.Create("VVAR3", 25.91M, 8),
        TrendBuilder.Create("SANB11", 40.77M, 7),
        TrendBuilder.Create("TORO4", 115.98M, 6),
    };
    public List<Trend> BestTrend(int listSize)
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