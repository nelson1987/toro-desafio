namespace Toro.Core.Repositories;

public class AccountRepository
{
    private readonly List<BankAccount> accounts = new List<BankAccount>() {
        new BankAccount() { Document ="45358996060", Account = "300123", Branch = "0001", Amount = 0  }
    };

    public async Task<BankAccount?> GetAccountBy(Func<BankAccount, bool> filter)
    {
        return await Task.FromResult(accounts.FirstOrDefault(filter));
    }

    public async Task Update(BankAccount conta)
    {
        var accountList = accounts;
        accountList.Remove(accountList.First(x => x.Account == conta.Account));
        accountList.Add(conta);
    }
}