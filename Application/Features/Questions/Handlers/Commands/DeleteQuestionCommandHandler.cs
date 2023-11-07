using Application.Contracts.Persistence;
using Application.Contracts.Persistence.Base;
using Application.Exceptions;
using Application.Features.Questions.Requests.Commands;
using Application.Features.Questions.Validators;
using CSharpFunctionalExtensions;
using Domain.Games;
using MediatR;

namespace Application.Features.Questions.Handlers.Commands;

public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IQuestionRepository _questionRepository;

    public DeleteQuestionCommandHandler(IUnitOfWork unitOfWork, IQuestionRepository questionRepository)
    {
        _unitOfWork = unitOfWork;
        _questionRepository = questionRepository;
    }

    public async Task Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var validator = new DeleteQuestionCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new QuizValidationException("Some validation error occurs", validationResult.Errors);

        Maybe<Question?> question = await _questionRepository.Get(Guid.Parse(request.QuestionId));
        if (question.HasNoValue)
            throw new QuizValidationException("Some validation error occurs", "questionId", "Question id does not exist");

        _questionRepository.Delete(question.Value!);
        await _unitOfWork.Save();
    }
}
