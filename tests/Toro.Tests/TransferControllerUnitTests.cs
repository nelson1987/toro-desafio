using Microsoft.AspNetCore.Mvc;
using Toro.Api.Controllers;

namespace Toro.Tests;

public class TransferControllerUnitTests
{
    private readonly TransferController _transferController;
    public TransferControllerUnitTests()
    {
        _transferController = new TransferController();
    }

    [Fact]
    public async void Given_Request_Valid_Return_Created()
    {
        var request = new CreateTransferCommand()
        {
            Event = "TRANSFER",
            Target = new CreateTransferTarget
            {
                Bank = "352",
                Branch = "0001",
                Account = "300123"
            },
            Origin = new CreateTransferOrigin
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
    public async void Given_Request_Null_Return_BadRequest()
    {
        IActionResult result = await _transferController.Post(null);
        Assert.NotNull(result);
        var response = (BadRequestResult)result;
        Assert.Equal(400, response.StatusCode);
    }

    [Fact]
    public async void Given_Request_Invalid_Return_UnprocessableEntity()
    {
        var request = new CreateTransferCommand() { };
        IActionResult result = await _transferController.Post(request);
        Assert.NotNull(result);
        var response = (UnprocessableEntityResult)result;
        Assert.Equal(422, response.StatusCode);
    }
}