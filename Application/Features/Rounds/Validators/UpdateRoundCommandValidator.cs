using Application.Features.Rounds.Requests.Commands;
using Application.Helpers;
using FluentValidation;

namespace Application.Features.Rounds.Validators;

public class UpdateRoundCommandValidator : AbstractValidator<UpdateRoundCommand>
{
    public UpdateRoundCommandValidator()
    {
        RuleFor(crc => crc.RoundUpdateDTO.Id)
            .ValidGuid()
            .OverridePropertyName("roundId");

        RuleFor(crc => crc.RoundUpdateDTO.GameId)
            .ValidGuid()
            .OverridePropertyName("gameId");

        RuleFor(urc => urc.RoundUpdateDTO.RoundNumber)
            .GreaterThan(0)
            .LessThanOrEqualTo(10)
            .OverridePropertyName("roundNumber");

        RuleFor(crc => crc.RoundUpdateDTO.RoundName)
            .NotEmpty()
            .OverridePropertyName("roundName");

        RuleFor(crc => crc.RoundUpdateDTO.RoundType)
            .ValidRoundType()
            .OverridePropertyName("roundType");
    }
}