using Toro.Core.Entities;
using Toro.Core.Features.Trends;

namespace Toro.Api.Controllers;
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
