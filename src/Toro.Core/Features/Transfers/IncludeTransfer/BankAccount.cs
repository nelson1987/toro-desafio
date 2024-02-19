namespace Toro.Core;

public class BankAccount
{
    public string Branch { get; set; }
    public string Account { get; set; }
    public string Document { get; set; }
    public decimal Amount { get; set; }

    public void Deposit(decimal amount)
    {
        Amount += amount;
    }
}