using Application.Contracts.Persistence.Base;
using Application.Features.Questions.Requests.Commands;
using MediatR;

namespace Application.Features.Questions.Handlers.Commands;

public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateQuestionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _unitOfWork.QuestionRepository.Get(Guid.Parse(request.QuestionUpdateDTO.Id));

        if (question is not null)
        {
            question.QuestionNumber = request.QuestionUpdateDTO.QuestionNumber;
            question.FullScore = request.QuestionUpdateDTO.FullScore;
            question.QuestionText = request.QuestionUpdateDTO.QuestionText;

            _unitOfWork.QuestionRepository.Update(question);
            await _unitOfWork.Save();
        }
    }
}
