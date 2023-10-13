using Application.Contracts.Persistence;
using Application.Features.Questions.Requests.Commands;
using FluentValidation;

namespace Application.Features.Questions.Validators;

public class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
{
    private readonly IQuestionRepository _questionRepository;

    public CreateQuestionCommandValidator(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;

        RuleFor(cqc => cqc.QuestionRequestDTO.FullScore)
            .GreaterThan(0)
            .LessThanOrEqualTo(5)
            .OverridePropertyName("fullScore");

        RuleFor(cqc => cqc.QuestionRequestDTO.QuestionText)
            .NotEmpty()
            .OverridePropertyName("questionText");
    }
}