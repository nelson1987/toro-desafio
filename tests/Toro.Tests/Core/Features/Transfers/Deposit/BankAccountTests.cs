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

    public class CreateMovementCommandTests
    {
        [Fact]
        public async Task Good_Path()
        {
            throw new NotImplementedException();
        }
    }
    public class CreateMovementOriginTests
    {
        [Fact]
        public async Task Good_Path()
        {
            throw new NotImplementedException();
        }
    }
    public class CreateMovementTargetTests
    {
        [Fact]
        public async Task Good_Path()
        {
            throw new NotImplementedException();
        }
    }
    public class MovementTypeTests
    {
        [Fact]
        public async Task Good_Path()
        {
            throw new NotImplementedException();
        }
    }
}
