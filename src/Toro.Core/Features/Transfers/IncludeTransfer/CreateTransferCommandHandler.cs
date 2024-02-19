using Toro.Core.Repositories;

namespace Toro.Core;

public class CreateTransferCommandHandler
{
    private readonly AccountRepository _accountRepository;

    public CreateTransferCommandHandler(AccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<bool> CanHandle(CreateMovementCommand command)
    {
        if (command.Target.Bank != "352") { return false; }
        if (await GetBankAccount(command) == null) { return false; }
        return true;
    }

    public async Task Handle(CreateMovementCommand command)
    {
        var conta = await GetBankAccount(command);
        if (conta != null)
        {
            conta.Deposit(command.Amount);
            await _accountRepository.Update(conta);
        }
    }

    private async Task<BankAccount?> GetBankAccount(CreateMovementCommand command)
    {
        Func<BankAccount, bool> originHasToroAccount = x =>
            x.Document == command.Origin.Cpf &&
            x.Branch == command.Target.Branch &&
            x.Account == command.Target.Account;
        return await _accountRepository.GetAccountBy(originHasToroAccount);
    }
}