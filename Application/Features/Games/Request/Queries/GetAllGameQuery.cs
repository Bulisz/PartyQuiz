using Application.DTOs;
using MediatR;

namespace Application.Features.Games.Request.Queries;

public class GetAllGameQuery : IRequest<List<GameResponseDTO>>
{
}
