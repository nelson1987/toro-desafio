using Microsoft.AspNetCore.Mvc;
using Toro.Api.Controllers;
using Toro.Core.Entities;
using Toro.Core.Features.Orders;
using Toro.Core.Repositories;

namespace Toro.Tests.Api;

public class OrderControllerUnitTests
{
    private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());
    private readonly OrderController _orderController;
    private readonly CreateOrderCommand _createOrderCommand;
    private readonly Mock<ITrendRepository> _trendRepository;
    private readonly Mock<IAccountRepository> _accountRepository;
    public OrderControllerUnitTests()
    {
        int lengthList = 5;
        List<Trend> trends = _fixture.Build<Trend>().CreateMany(lengthList).ToList();
        BankAccount account = _fixture
            .Build<BankAccount>()
            .With(x => x.Account, "300123")
            .With(x => x.Amount, 1000000)
            .Create();

        _trendRepository = _fixture.Freeze<Mock<ITrendRepository>>();
        _trendRepository
             .Setup(x => x.BestTrend(lengthList))
             .Returns(Task.FromResult(trends));
        _trendRepository
             .Setup(x => x.GetBySymbol(trends.FirstOrDefault()!.Symbol))
             .Returns(Task.FromResult(trends.FirstOrDefault()));
        _accountRepository = _fixture.Freeze<Mock<IAccountRepository>>();
        _accountRepository
             .Setup(x => x.GetAccountBy(It.IsAny<Func<BankAccount, bool>>()))
             .Returns(Task.FromResult(account)!);

        _createOrderCommand = _fixture.Build<CreateOrderCommand>()
            .With(x => x.Symbol, trends.FirstOrDefault()!.Symbol)
            .Create();
        _orderController = _fixture.Build<OrderController>()
            .OmitAutoProperties()
            .Create();
    }

    [Fact]
    public async Task Given_Request_Valid_Return_Create()
    {
        IActionResult result = await _orderController.Post(_createOrderCommand);
        Assert.NotNull(result);
        var response = (StatusCodeResult)result;
        Assert.Equal(201, response.StatusCode);
    }
}
