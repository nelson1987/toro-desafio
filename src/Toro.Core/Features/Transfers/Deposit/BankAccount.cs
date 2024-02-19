namespace Toro.Core.Features.Transfers.Deposit;

public class BankAccount
{
    public string Branch { get; set; }
    public string Account { get; set; }
    public string Document { get; set; }
    public decimal Amount { get; set; }

    public void Deposit(decimal amount)
    {
        if (amount <= 0) throw new ArgumentException("Amount have been greater than 0");
        Amount += amount;
    }
}