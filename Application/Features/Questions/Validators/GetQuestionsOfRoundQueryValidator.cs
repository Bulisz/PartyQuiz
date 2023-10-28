using Application.Features.Questions.Requests.Queries;
using Application.Helpers;
using FluentValidation;

namespace Application.Features.Questions.Validators;

public class GetQuestionsOfRoundQueryValidator : AbstractValidator<GetQuestionsOfRoundQuery>
{
    public GetQuestionsOfRoundQueryValidator()
    {
        RuleFor(gqor => gqor.RoundId)
            .ValidGuid()
            .OverridePropertyName("gameId");
    }
}