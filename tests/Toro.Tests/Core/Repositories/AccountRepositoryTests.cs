using Toro.Core.Entities;
using Toro.Core.Repositories;

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
            throw new NotImplementedException();
        }
    }
}
