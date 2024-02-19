namespace Toro.Core.Entities
{
    public class Trend
    {
        public string Symbol { get; set; }
        public decimal CurrentPrice { get; set; }
        public int Buys { get; set; }
    }
}
