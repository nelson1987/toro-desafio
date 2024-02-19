namespace Toro.Core.Entities;

public class BankAccount
{
    public string Branch { get; set; }
    public string Account { get; set; }
    public string Document { get; set; }
    public decimal Amount { get; set; }
    public List<Order> Orders { get; set; }

    public void Deposit(decimal amount)
    {
        if (amount <= 0) throw new ArgumentException("Amount have been greater than 0");
        Amount += amount;
    }
    public void BuyOrder(Trend trend, int amount)
    {
        var operationValue = trend.CurrentPrice * amount;
        if (Amount < operationValue) throw new ArgumentException("Amount have been greater than Operation Value");
        InsertOrder(new Order() { Symbol = trend.Symbol, Amount = amount });
        Amount -= operationValue;
    }
    private void InsertOrder(Order order)
    {
        var existentOrder = Orders.FirstOrDefault(x => x.Symbol == order.Symbol);
        if (Orders.Any(x => x.Symbol == order.Symbol))
        {
            order.Amount += existentOrder.Amount;
            Orders.Remove(existentOrder);
        }
        Orders.Add(order);

    }
}