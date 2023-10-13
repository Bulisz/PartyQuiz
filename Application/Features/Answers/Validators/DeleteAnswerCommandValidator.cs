using Application.Contracts.Persistence;
using Application.Features.Answers.Requests.Commands;
using Application.Helpers;
using FluentValidation;

namespace Application.Features.Answers.Validators;

public class DeleteAnswerCommandValidator : AbstractValidator<DeleteAnswerCommand>
{
    private readonly IAnswerRepository _answerRepository;

    public DeleteAnswerCommandValidator(IAnswerRepository answerRepository)
    {
        _answerRepository = answerRepository;

        RuleFor(dac => dac.AnswerId)
            .Cascade(CascadeMode.Stop)
            .ValidGuid()
            .MustAsync(async (ai, token) => await _answerRepository.Exists(Guid.Parse(ai)))
                .WithMessage("{PropertyValue} nem létezik")
            .OverridePropertyName("answerId");
    }
}