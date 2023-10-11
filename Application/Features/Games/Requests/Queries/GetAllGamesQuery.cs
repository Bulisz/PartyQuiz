using Application.DTOs;
using MediatR;

namespace Application.Features.Games.Requests.Queries;

public class GetAllGamesQuery : IRequest<List<GameResponseDTO>>
{
}
