using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Toro.Api.Controllers;
using Toro.Core.Features.Transfers.Deposit;

namespace Toro.Tests.Api;
public class TransferControllerUnitTests
{
    private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());
    private readonly TransferController _transferController;
    private readonly CreateMovementCommand _request;
    private readonly CreateMovementOrigin _origin;
    private readonly Mock<IValidator<CreateMovementCommand>> _validator;
    private readonly Mock<ICreateMovementHandler> _handler;

    public TransferControllerUnitTests()
    {
        var target = _fixture.Build<CreateMovementTarget>()
            .With(x => x.Bank, "352")
            .With(x => x.Branch, "0001")
            .With(x => x.Account, "300123")
            .Create();

        _origin = _fixture.Build<CreateMovementOrigin>()
            .With(x => x.Cpf, "45358996060")
            .Create();

        _request = _fixture.Build<CreateMovementCommand>()
            .With(x => x.Event, MovementType.TRANSFER)
            .With(x => x.Target, target)
            .With(x => x.Origin, _origin)
            .Create();

        _validator = _fixture.Freeze<Mock<IValidator<CreateMovementCommand>>>();
        _validator
             .Setup(x => x.Validate(_request))
             .Returns(new ValidationResult());

        _handler = _fixture.Freeze<Mock<ICreateMovementHandler>>();
        _handler
             .Setup(x => x.CanHandle(_request))
             .Returns(Task.FromResult(true));

        _transferController = _fixture.Build<TransferController>()
            .OmitAutoProperties()
            .Create();
    }

    [Fact]
    public async Task Given_Request_Valid_Return_Created()
    {
        IActionResult result = await _transferController.Post(_request);
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
        _validator
             .Setup(x => x.Validate(_request))
             .Returns(new ValidationResult(
                 new ValidationFailure[] {
                 new ValidationFailure("", "")
             }));

        IActionResult result = await _transferController.Post(_request);
        Assert.NotNull(result);
        var response = (UnprocessableEntityResult)result;
        Assert.Equal(422, response.StatusCode);
    }

    [Fact]
    public async Task Given_Request_InvalidAccount_Return_UnprocessableEntity()
    {
        var request = _request with
        {
            Origin = _fixture.Build<CreateMovementOrigin>()
            .With(x => x.Cpf, "55559996666")
            .Create()
        };
        IActionResult result = await _transferController.Post(request);
        Assert.NotNull(result);
        var response = (UnprocessableEntityResult)result;
        Assert.Equal(422, response.StatusCode);
    }
}