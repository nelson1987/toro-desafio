using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Toro.Api.Controllers;
using Toro.Core.Features.Transfers.Deposit;
using Toro.Core.Repositories;

namespace Toro.Tests.Api;
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