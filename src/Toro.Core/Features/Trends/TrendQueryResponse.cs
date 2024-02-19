namespace Toro.Core.Features.Trends
{
    public record TrendQueryResponse
    {
        public string Symbol { get; set; }
        public decimal CurrentPrice { get; set; }
    }
}
