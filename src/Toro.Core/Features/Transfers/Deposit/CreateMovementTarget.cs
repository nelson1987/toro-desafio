using FluentValidation;

namespace Toro.Core.Features.Transfers.Deposit;

public record CreateMovementTarget
{
    public required string Bank { get; set; }
    public required string Branch { get; set; }
    public required string Account { get; set; }
}

public class CreateMovementTargetValidator : AbstractValidator<CreateMovementTarget>
{
    public CreateMovementTargetValidator()
    {
        RuleFor(x => x.Bank).NotEmpty().NotNull();
        RuleFor(x => x.Branch).NotEmpty().NotNull();
        RuleFor(x => x.Account).NotEmpty().NotNull();
    }
}