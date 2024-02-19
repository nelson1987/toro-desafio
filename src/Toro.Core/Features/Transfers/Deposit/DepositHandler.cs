using Toro.Core.Entities;
using Toro.Core.Repositories;
using Toro.Core.Utils;

namespace Toro.Core.Features.Transfers.Deposit;
public interface ICreateMovementHandler
{
    Task<bool> CanHandle(CreateMovementCommand command);

    Task Handle(CreateMovementCommand command);
}
public class DepositHandler : ICreateMovementHandler
{
    private readonly IAccountRepository _accountRepository;

    public DepositHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<bool> CanHandle(CreateMovementCommand command)
    {
        return
            await GetValidBankAccount(command) != null &&
            command.Event == MovementType.TRANSFER &&
            command.Target.Bank == ToroConstants.BANK_NUMBER;
    }

    public async Task Handle(CreateMovementCommand command)
    {
        var conta = await GetValidBankAccount(command);
        if (conta != null)
        {
            conta.Deposit(command.Amount);
            await _accountRepository.Update(conta);
        }
    }

    private async Task<BankAccount?> GetValidBankAccount(CreateMovementCommand command)
    {
        Func<BankAccount, bool> originHasToroAccount = x =>
            x.Document == command.Origin.Cpf &&
            x.Branch == command.Target.Branch &&
            x.Account == command.Target.Account;
        return await _accountRepository.GetAccountBy(originHasToroAccount);
    }
}