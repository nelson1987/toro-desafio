using AutoFixture.AutoMoq;
using AutoFixture;
using Toro.Core.Repositories;
using Moq;
using Toro.Core.Features.Transfers.Deposit;

namespace Toro.Tests.Core.Repositories
{
    public class AccountRepositoryTests
    {
        private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());

        private readonly Mock<AccountRepository> _repository;
        private readonly BankAccount _account;

        public AccountRepositoryTests()
        {
            _account = _fixture.Build<BankAccount>()
                .Create();
            _repository = _fixture.Freeze<Mock<AccountRepository>>();
            _repository
                 .Setup(x => x.GetAccountBy(It.IsAny<Func<BankAccount, bool>>()))
                 .Returns(Task.FromResult(_account)!);
        }

        [Fact]
        public async Task Good_Path()
        {
            //_repository.GetAccountBy
            //_repository.GetAccountBy(x=>x.Account)
            throw new NotImplementedException();
        }
        /*
         
    Task<BankAccount?> GetAccountBy(Func<BankAccount, bool> filter);
    Task Update(BankAccount conta);
         */
    }
}
