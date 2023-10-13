using Application.Contracts.Persistence;
using Application.Features.Questions.Requests.Commands;
using Application.Helpers;
using FluentValidation;

namespace Application.Features.Questions.Validators;

public class DeleteQuestionCommandValidator: AbstractValidator<DeleteQuestionCommand>
{
    private readonly IQuestionRepository _questionRepository;

    public DeleteQuestionCommandValidator(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;

        RuleFor(dqc => dqc.QuestionId)
            .Cascade(CascadeMode.Stop)
            .ValidGuid()
            .MustAsync(async (qi, token) => await _questionRepository.Exists(Guid.Parse(qi)))
                .WithMessage("{PropertyValue} nem létezik")
            .OverridePropertyName("questionId");
    }
}