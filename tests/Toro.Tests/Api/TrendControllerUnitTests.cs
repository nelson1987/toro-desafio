using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Mvc;
using Toro.Api.Controllers;

namespace Toro.Tests.Api;

public class TrendControllerUnitTests
{
    private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());
    private readonly TrendController _trendController;

    //private readonly Mock<AccountRepository> _repository;
    //private readonly BankAccount _account;

    public TrendControllerUnitTests()
    {
        _trendController = _fixture.Build<TrendController>().Create();
        //_account = _fixture.Build<BankAccount>()
        //    .Create();
        //_repository = _fixture.Freeze<Mock<AccountRepository>>();
        //_repository
        //     .Setup(x => x.GetAccountBy(It.IsAny<Func<BankAccount, bool>>()))
        //     .Returns(Task.FromResult(_account)!);
    }

    [Fact]
    public async Task Given_Request_Valid_Without_Size_Return_5_Ok()
    {
        IActionResult result = await _trendController.Get();
        Assert.NotNull(result);
        var response = (OkResult)result;
        Assert.Equal(200, response.StatusCode);
    }

    [Fact]
    public async Task Given_Request_Valid_Size_3_Return_3_Ok()
    {
        IActionResult result = await _trendController.Get(3);
        Assert.NotNull(result);
        var response = (OkResult)result;
        Assert.Equal(200, response.StatusCode);
    }
}
