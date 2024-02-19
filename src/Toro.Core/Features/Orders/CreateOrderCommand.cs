namespace Toro.Core.Features.Orders
{
    public record CreateOrderCommand
    {
        public string Symbol { get; set; }
        public int Amount { get; set; }
    }
}
