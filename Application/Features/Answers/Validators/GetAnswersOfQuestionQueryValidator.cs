using Application.Features.Answers.Requests.Queries;
using Application.Helpers;
using FluentValidation;

namespace Application.Features.Answers.Validators;

public class GetAnswersOfQuestionQueryValidator : AbstractValidator<GetAnswersOfQuestionQuery>
{
    public GetAnswersOfQuestionQueryValidator()
    {
        RuleFor(gaq => gaq.QuestionId)
            .ValidGuid()
            .OverridePropertyName("questionId");
    }
}