using Application.Contracts.Persistence.Base;
using Application.Exceptions;
using Application.Features.Games.Requests.Commands;
using Application.Features.Games.Validators;
using MediatR;

namespace Application.Features.Games.Handlers.Commands;

public class UpdateGameCommandHandler : IRequestHandler<UpdateGameCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateGameCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateGameCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateGameCommandValidator(_unitOfWork.GameRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new QuizValidationException("Some vaidation error occcurs", validationResult.Errors);

        var game = await _unitOfWork.GameRepository.Get(Guid.Parse(request.GameUpdateDTO.Id));

        if (game is not null)
        {
            game.GameName = request.GameUpdateDTO.GameName;

            _unitOfWork.GameRepository.Update(game);
            await _unitOfWork.Save();
        }
    }
}
