using Toro.Core.Entities;

namespace Toro.Tests.Core.Features.Transfers.Deposit
{
    public class BankAccountTests
    {
        private readonly BankAccount _bankAccount;

        public BankAccountTests()
        {
            _bankAccount = new BankAccount();
        }

        [Fact]
        public async Task ToDeposit_Success()
        {
            _bankAccount.Amount = 100;
            _bankAccount.Deposit(100);
            Assert.Equal(200, _bankAccount.Amount);
        }

        [Fact]
        public async Task ToDeposit_Value_Zero_Raise_Exception()
        {
            _bankAccount.Amount = 100;
            Assert.Throws<ArgumentException>(() => _bankAccount.Deposit(0));
        }
    }
}
