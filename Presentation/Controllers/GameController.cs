using Application.DTOs;
using Application.Features.Games.Request.Commands;
using Application.Features.Games.Request.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("partyquiz/[controller]")]
public class GameController : ControllerBase
{
    private readonly IMediator _mediator;

    public GameController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(nameof(GetAllGames))]
    public async Task<ActionResult<IEnumerable<GameResponseDTO>>> GetAllGames()
    {
        var games = await _mediator.Send(new GetAllGameQuery());
        return Ok(games);
    }

    [HttpPost(nameof(CreateGame))]
    public async Task<ActionResult<GameResponseDTO>> CreateGame(GameRequestDTO gameRequestDTO)
    {
        var game = await _mediator.Send(new CreateGameCommand() { GameRequestDTO = gameRequestDTO });
        return Ok(game);
    }
}
