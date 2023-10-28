using Application.DTOs;
using MediatR;

namespace Application.Features.Games.Requests.Queries;

public class GetAllGameNamesQuery : IRequest<List<GameResponseDTO>>
{
}
