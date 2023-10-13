using Application.Contracts.Persistence;
using Application.Features.Rounds.Requests.Commands;
using Application.Helpers;
using FluentValidation;

namespace Application.Features.Rounds.Validators;

public class DeleteRoundCommandValidator : AbstractValidator<DeleteRoundCommand>
{
    private readonly IRoundRepository _roundrepository;

    public DeleteRoundCommandValidator(IRoundRepository roundrepository)
    {
        _roundrepository = roundrepository;

        RuleFor(drc => drc.RoundId)
                .Cascade(CascadeMode.Stop)
                .ValidGuid()
                .MustAsync(async (gi, token) => await _roundrepository.Exists(Guid.Parse(gi)))
                    .WithMessage("{PropertyValue} nem létezik")
                .OverridePropertyName("roundId");
    }
}