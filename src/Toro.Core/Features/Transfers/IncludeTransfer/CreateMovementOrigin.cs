using FluentValidation;

namespace Toro.Core;

public record CreateMovementOrigin
{
    public string Bank { get; set; }
    public string Branch { get; set; }
    public string Cpf { get; set; }
}

public class CreateMovementOriginValidator : AbstractValidator<CreateMovementOrigin>
{
    public CreateMovementOriginValidator()
    {
        RuleFor(x => x.Bank).NotEmpty().NotNull();
        RuleFor(x => x.Branch).NotEmpty().NotNull();
        RuleFor(x => x.Cpf).NotEmpty().NotNull();
    }
}