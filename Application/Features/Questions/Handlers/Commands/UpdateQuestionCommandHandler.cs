using Application.Contracts.Persistence;
using Application.Contracts.Persistence.Base;
using Application.Exceptions;
using Application.Features.Questions.Requests.Commands;
using Application.Features.Questions.Validators;
using CSharpFunctionalExtensions;
using Domain.Games;
using MediatR;

namespace Application.Features.Questions.Handlers.Commands;

public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IQuestionRepository _questionRepository;

    public UpdateQuestionCommandHandler(IUnitOfWork unitOfWork, IQuestionRepository questionRepository)
    {
        _unitOfWork = unitOfWork;
        _questionRepository = questionRepository;
    }

    public async Task Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateQuestionCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new QuizValidationException("Some vaidation error occcurs", validationResult.Errors);

        Maybe<Question?> question = await _questionRepository.Get(Guid.Parse(request.QuestionUpdateDTO.Id));
        if (question.HasNoValue)
            throw new QuizValidationException("Some validation error occurs", "questionId", "Question id does not exist");

        question.Value!.Modify(request.QuestionUpdateDTO.FullScore, request.QuestionUpdateDTO.QuestionText);

        _questionRepository.Update(question.Value!);
        await _unitOfWork.Save();
    }
}
