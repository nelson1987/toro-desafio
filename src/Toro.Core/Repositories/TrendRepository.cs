using MongoDB.Driver;
using Toro.Core.Entities;
using Toro.Core.Utils;

namespace Toro.Api.Controllers;
public interface ITrendRepository
{
    Task<List<Trend>> BestTrend(int listSize = 5);
    Task<Trend?> GetBySymbol(string symbol);
}
public class TrendRepository : ITrendRepository
{
    private readonly IMongoContext _context;

    public TrendRepository(IMongoContext context)
    {
        _context = context;
    }
    public async Task<List<Trend>> BestTrend(int listSize = 5)
    {
        return await Task.FromResult(_context.Transacoes.AsQueryable().OrderByDescending(x => x.Buys).Take(listSize).ToList());
    }
    public async Task<Trend?> GetBySymbol(string symbol)
    {
        return await Task.FromResult(_context.Transacoes.AsQueryable().FirstOrDefault(x => x.Symbol == symbol));
    }
}
