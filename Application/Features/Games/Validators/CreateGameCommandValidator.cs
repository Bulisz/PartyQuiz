using Application.Features.Games.Requests.Commands;
using FluentValidation;

namespace Application.Features.Games.Validators;

public class CreateGameCommandValidator : AbstractValidator<CreateGameCommand>
{
    public CreateGameCommandValidator()
    {
        RuleFor(cgc => cgc.GameRequestDTO.GameName)
            .NotEmpty()
            .OverridePropertyName("gameName");
    }
}
