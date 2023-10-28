using Application.Features.Rounds.Requests.Commands;
using Application.Helpers;
using FluentValidation;

namespace Application.Features.Rounds.Validators;

public class DeleteRoundCommandValidator : AbstractValidator<DeleteRoundCommand>
{
    public DeleteRoundCommandValidator()
    {
        RuleFor(drc => drc.RoundId)
                .ValidGuid()
                .OverridePropertyName("roundId");
    }
}