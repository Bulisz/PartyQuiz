using Application.Contracts.Persistence;
using Application.Features.Questions.Requests.Commands;
using FluentValidation;

namespace Application.Features.Questions.Validators;

public class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
{
    private readonly IQuestionRepository _questionRepository;

    public UpdateQuestionCommandValidator(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;

        RuleFor(uqc => uqc.QuestionUpdateDTO.FullScore)
            .GreaterThan(0)
            .LessThanOrEqualTo(5)
            .OverridePropertyName("fullScore");

        RuleFor(uqc => uqc.QuestionUpdateDTO.QuestionText)
            .NotEmpty()
            .OverridePropertyName("questionText");
    }
}