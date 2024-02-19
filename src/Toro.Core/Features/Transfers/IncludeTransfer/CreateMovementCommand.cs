using FluentValidation;

namespace Toro.Core;

public record CreateMovementCommand
{
    public MovementType Event { get; set; }
    public CreateMovementTarget Target { get; set; }
    public CreateMovementOrigin Origin { get; set; }
    public decimal Amount { get; set; }
}

public class CreateMovementCommandValidator : AbstractValidator<CreateMovementCommand>
{
    public CreateMovementCommandValidator()
    {
        RuleFor(x => x.Event).IsInEnum().NotEmpty().NotNull();
        RuleFor(x => x.Target).SetValidator(new CreateMovementTargetValidator());
        RuleFor(x => x.Origin).SetValidator(new CreateMovementOriginValidator());
        RuleFor(x => x.Amount).NotEmpty().NotNull().GreaterThan(0);
    }
}