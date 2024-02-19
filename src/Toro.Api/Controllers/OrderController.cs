using Microsoft.AspNetCore.Mvc;
using Toro.Core.Repositories;

namespace Toro.Api.Controllers;

[ApiController]
[Route("[Controller]")]
public class OrderController : ControllerBase
{
    private readonly ITrendRepository _trendRepository;
    private readonly IAccountRepository _accountRepository;
    public OrderController(ITrendRepository trendRepository, IAccountRepository accountRepository)
    {
        _trendRepository = trendRepository;
        _accountRepository = accountRepository;
    }
    [HttpPost("/orders")]
    public async Task<IActionResult> Post(CreateOrderCommand command)
    {
        var trend = await _trendRepository.GetBySymbol(command.Symbol);
        if (trend == null) { return NotFound(); }

        var user = await _accountRepository.GetAccountBy(x => x.Account == "300123");
        if (user == null) { return NotFound(); }

        user.BuyOrder(trend!, command.Amount);

        return StatusCode(201);
    }
}
