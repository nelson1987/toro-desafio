using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Toro.Core.Features.Transfers.Deposit;

namespace Toro.Api.Controllers;

[ApiController]
[Route("[Controller]")]
public class TransferController : ControllerBase
{
    private readonly ICreateMovementHandler _handler;
    private readonly IValidator<CreateMovementCommand> _validator;

    public TransferController(ICreateMovementHandler handler,
            IValidator<CreateMovementCommand> validator)
    {
        _handler = handler;
        _validator = validator;
    }

    [HttpPost("/spb/events")]
    public async Task<IActionResult> Post(CreateMovementCommand command)
    {
        if (command == null)
            return BadRequest();

        if (!_validator.Validate(command).IsValid)
            return UnprocessableEntity();

        if (!await _handler.CanHandle(command))
            return UnprocessableEntity();

        await _handler.Handle(command);

        return StatusCode(201);
    }
}