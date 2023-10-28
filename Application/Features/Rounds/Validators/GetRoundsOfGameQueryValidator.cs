using Application.Features.Rounds.Requests.Queries;
using Application.Helpers;
using FluentValidation;

namespace Application.Features.Rounds.Validators;

public class GetRoundsOfGameQueryValidator : AbstractValidator<GetRoundsOfGameQuery>
{
    public GetRoundsOfGameQueryValidator()
    {
        RuleFor(grogq => grogq.GameId)
            .ValidGuid()
            .OverridePropertyName("gameId");
    }
}