using Application.Features.Rounds.Requests.Commands;
using Application.Helpers;
using FluentValidation;

namespace Application.Features.Rounds.Validators;

public class CreateRoundCommandValidator : AbstractValidator<CreateRoundCommand>
{

    public CreateRoundCommandValidator()
    {
        RuleFor(crc => crc.RoundRequestDTO.GameId)
            .ValidGuid()
            .OverridePropertyName("gameId");

        RuleFor(crc => crc.RoundRequestDTO.RoundType)
            .ValidRoundType()
            .OverridePropertyName("roundType");

        RuleFor(crc => crc.RoundRequestDTO.RoundNumber)
            .GreaterThan(0)
            .LessThanOrEqualTo(10)
            .OverridePropertyName("roundNumber");

        RuleFor(crc => crc.RoundRequestDTO.RoundName)
            .NotEmpty()
            .OverridePropertyName("roundName");
        
    }
}