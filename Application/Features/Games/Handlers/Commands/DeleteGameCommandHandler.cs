using Application.Contracts.Persistence.Base;
using Application.Exceptions;
using Application.Features.Games.Requests.Commands;
using Application.Features.Games.Validators;
using MediatR;

namespace Application.Features.Games.Handlers.Commands;

public class DeleteGameCommandHandler : IRequestHandler<DeleteGameCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteGameCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteGameCommand request, CancellationToken cancellationToken)
    {
        var validator = new DeleteGameCommandValidator(_unitOfWork.GameRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new QuizValidationException("Some vaidation error occcurs", validationResult.Errors);

        var game = await _unitOfWork.GameRepository.Get(Guid.Parse(request.GameId));

        _unitOfWork.GameRepository.Delete(game);
        await _unitOfWork.Save();

    }
}
