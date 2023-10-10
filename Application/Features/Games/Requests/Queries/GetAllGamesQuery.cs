using Application.DTOs;
using MediatR;

namespace Application.Features.Games.Request.Queries;

public class GetAllGamesQuery : IRequest<List<GameResponseDTO>>
{
}
