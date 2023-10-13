using Application.Contracts.Persistence.Base;
using Application.Exceptions;
using Application.Features.Rounds.Requests.Commands;
using Application.Features.Rounds.Validators;
using MediatR;

namespace Application.Features.Rounds.Handlers.Commands;

public class UpdateRoundCommandHandler : IRequestHandler<UpdateRoundCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRoundCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateRoundCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateRoundCommandValidator(_unitOfWork.RoundRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new QuizValidationException("Some vaidation error occcurs", validationResult.Errors);

        var round = await _unitOfWork.RoundRepository.Get(Guid.Parse(request.RoundUpdateDTO.Id));

        if (round is not null)
        {
            round.Modify(request.RoundUpdateDTO.RoundNumber, request.RoundUpdateDTO.RoundName, request.RoundUpdateDTO.RoundType);

            _unitOfWork.RoundRepository.Update(round);
            await _unitOfWork.Save();
        }
    }
}
