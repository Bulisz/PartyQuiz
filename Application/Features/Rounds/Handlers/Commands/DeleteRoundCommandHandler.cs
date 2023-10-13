using Application.Contracts.Persistence.Base;
using Application.Exceptions;
using Application.Features.Rounds.Validators;
using Application.Features.Rounds.Requests.Commands;
using MediatR;

namespace Application.Features.Rounds.Handlers.Commands;

public class DeleteRoundCommandHandler : IRequestHandler<DeleteRoundCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRoundCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteRoundCommand request, CancellationToken cancellationToken)
    {
        var validator = new DeleteRoundCommandValidator(_unitOfWork.RoundRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new QuizValidationException("Some vaidation error occcurs", validationResult.Errors);

        var round = await _unitOfWork.RoundRepository.Get(Guid.Parse(request.RoundId));

        _unitOfWork.RoundRepository.Delete(round!);
        await _unitOfWork.Save();
    }
}
