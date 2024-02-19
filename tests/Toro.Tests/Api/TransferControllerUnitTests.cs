using AutoFixture;
using AutoFixture.AutoMoq;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Principal;
using Toro.Api.Controllers;
using Toro.Core.Entities;
using Toro.Core.Features.Transfers.Deposit;
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
public class TransferControllerUnitTests
{
    private readonly TransferController _transferController;
    private readonly CreateMovementCommand _createTransferCommand;
    private readonly IValidator<CreateMovementCommand> _validator;

    public TransferControllerUnitTests()
    {
        _validator = new CreateMovementCommandValidator();
        var repository = new AccountRepository();
        var handler = new DepositHandler(repository);
        _transferController = new TransferController(handler, _validator);
        _createTransferCommand = new CreateMovementCommand();
    }

    [Fact]
    public async Task Given_Request_Valid_Return_Created()
    {
        var request = _createTransferCommand with
        {
            Event = MovementType.TRANSFER,
            Target = new CreateMovementTarget
            {
                Bank = "352",
                Branch = "0001",
                Account = "300123"
            },
            Origin = new CreateMovementOrigin
            {
                Bank = "033",
                Branch = "03312",
                Cpf = "45358996060"
            },
            Amount = 1000
        };
        IActionResult result = await _transferController.Post(request);
        Assert.NotNull(result);
        var response = (StatusCodeResult)result;
        Assert.Equal(201, response.StatusCode);
    }

    [Fact]
    public async Task Given_Request_Null_Return_BadRequest()
    {
        IActionResult result = await _transferController.Post(null);
        Assert.NotNull(result);
        var response = (BadRequestResult)result;
        Assert.Equal(400, response.StatusCode);
    }

    [Fact]
    public async Task Given_Request_Invalid_Return_UnprocessableEntity()
    {
        var request = _createTransferCommand with { };
        IActionResult result = await _transferController.Post(request);
        Assert.NotNull(result);
        var response = (UnprocessableEntityResult)result;
        Assert.Equal(422, response.StatusCode);
    }

    [Fact]
    public async Task Given_Request_InvalidAccount_Return_UnprocessableEntity()
    {
        var request = _createTransferCommand with
        {
            Event = MovementType.TRANSFER,
            Target = new CreateMovementTarget
            {
                Bank = "352",
                Branch = "0001",
                Account = "300123"
            },
            Origin = new CreateMovementOrigin
            {
                Bank = "033",
                Branch = "03312",
                Cpf = "55559996666"
            },
            Amount = 1000
        };
        IActionResult result = await _transferController.Post(request);
        Assert.NotNull(result);
        var response = (UnprocessableEntityResult)result;
        Assert.Equal(422, response.StatusCode);
    }
}