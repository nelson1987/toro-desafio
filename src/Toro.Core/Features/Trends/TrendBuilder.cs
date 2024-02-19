using Toro.Core.Entities;

namespace Toro.Core.Features.Trends
{
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
}
