using MongoDB.Driver;
using Toro.Core.Entities;
using Toro.Core.Utils;

namespace Toro.Core.Repositories;

public interface IAccountRepository
{
    Task<BankAccount?> GetAccountBy(Func<BankAccount, bool> filter);
    Task UpdateAmount(BankAccount conta);
}

public class AccountRepository : IAccountRepository
{
    private readonly IMongoContext _context;

    public AccountRepository(IMongoContext context)
    {
        _context = context;
    }

    public async Task<BankAccount?> GetAccountBy(Func<BankAccount, bool> filter)
    {
        return await Task.FromResult(_context.Contas.AsQueryable().FirstOrDefault(filter));
    }

    public async Task Update(BankAccount conta)
    {
        var filter = Builders<BankAccount>.Filter.Eq(x => x.Account, conta.Account);
        var update = Builders<BankAccount>.Update.Set(x => x, conta);
        await _context.Contas.UpdateOneAsync(filter, update);
    }
    public async Task UpdateAmount(BankAccount conta)
    {
        var filter = Builders<BankAccount>.Filter.Eq(x => x.Account, conta.Account);
        var update = Builders<BankAccount>.Update.Set(x => x.Amount, conta.Amount);
        await _context.Contas.UpdateOneAsync(filter, update);
    }
}