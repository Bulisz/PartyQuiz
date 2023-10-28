using Application.Features.Games.Requests.Commands;
using Application.Helpers;
using FluentValidation;

namespace Application.Features.Games.Validators;

public class UpdateGameCommandValidator : AbstractValidator<UpdateGameCommand>
{
    public UpdateGameCommandValidator()
    {
        RuleFor(ugc => ugc.GameUpdateDTO.Id)
            .ValidGuid()
            .OverridePropertyName("gameId");

        RuleFor(ugc => ugc.GameUpdateDTO.GameName)
            .NotEmpty()
            .OverridePropertyName("gameName");
    }
}
