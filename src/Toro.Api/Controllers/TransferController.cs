using Microsoft.AspNetCore.Mvc;

namespace Toro.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TransferController : ControllerBase
{
    public TransferController()
    {
        
    }
    [HttpPost("/spb/events")]
    public async Task<IActionResult> Post(CreateTransferCommand command)
    {
        if (command == null) { return BadRequest(); }
        if (command.Amount == 0) { return UnprocessableEntity(); }
        return StatusCode(201);
    }
}
public record CreateTransferCommand
{
    public string Event { get; set; }
    public CreateTransferTarget Target { get; set; }
    public CreateTransferOrigin Origin { get; set; }
    public decimal Amount { get; set; }
}
public record CreateTransferTarget
{
    public string Bank { get; set; }
    public string Branch { get; set; }
    public string Account { get; set; }
}
public record CreateTransferOrigin
{
    public string Bank { get; set; }
    public string Branch { get; set; }
    public string Cpf { get; set; }
}
