using Toro.Core.Features.Transfers.Deposit;
using Toro.Core.Repositories;

namespace Toro.Tests.Core.Features.Transfers.Deposit;

public class DepositHandlerTests
{
    private readonly DepositHandler _handler;
    private readonly CreateMovementCommand _createTransferCommand;

    public DepositHandlerTests()
    {
        var repository = new AccountRepository();
        _handler = new DepositHandler(repository);
        _createTransferCommand = new CreateMovementCommand()
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
    }

    [Fact]
    public async Task Given_Request_Valid_CanHandle_Return_True()
    {
        Assert.True(await _handler.CanHandle(_createTransferCommand));
    }

    [Fact]
    public async Task Given_Request_ValidAccount_CanHandle_Return_False()
    {
        var origin = new CreateMovementOrigin
        {
            Cpf = "12258886000"
        };
        var request = _createTransferCommand with
        {
            Origin = origin,
        };
        Assert.False(await _handler.CanHandle(request));
    }
}