using Application.Contracts.Persistence.Base;
using Application.Exceptions;
using Application.Features.Rounds.Validators;
using Application.Features.Rounds.Requests.Commands;
using MediatR;
using Application.Contracts.Persistence;
using CSharpFunctionalExtensions;
using Domain.Games;

namespace Application.Features.Rounds.Handlers.Commands;

public class DeleteRoundCommandHandler : IRequestHandler<DeleteRoundCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRoundRepository _roundRepository;

    public DeleteRoundCommandHandler(IUnitOfWork unitOfWork, IRoundRepository roundRepository)
    {
        _unitOfWork = unitOfWork;
        _roundRepository = roundRepository;
    }

    public async Task Handle(DeleteRoundCommand request, CancellationToken cancellationToken)
    {
        var validator = new DeleteRoundCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new QuizValidationException("Some validation error occurs", validationResult.Errors);

        Maybe<Round?> round = await _roundRepository.Get(Guid.Parse(request.RoundId));
        if (round.HasNoValue)
            throw new QuizValidationException("Some validation error occurs", "roundId", "Round Id does not exist");

        _roundRepository.Delete(round.Value!);
        await _unitOfWork.Save();
    }
}
