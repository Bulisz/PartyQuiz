using Application.DTOs;
using Domain.Entities;
using MediatR;

namespace Application.Features.Games.Request.Commands;

public class CreateGameCommand : IRequest<GameResponseDTO>
{
    public required GameRequestDTO GameRequestDTO { get; init; }
}
