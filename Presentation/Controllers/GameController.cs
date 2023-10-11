using Application.DTOs;
using Application.Features.Games.Requests.Queries;
using Application.Features.Games.Requests.Commands;
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

    [HttpGet("GetGameByName/{gameName}")]
    public async Task<ActionResult<GameResponseDTO>> GetGameByName(string gameName)
    {
        var game = await _mediator.Send(new GetGameByNameQuery(gameName));
        return Ok(game);
    }

    [HttpGet(nameof(GetAllGames))]
    public async Task<ActionResult<IEnumerable<GameResponseDTO>>> GetAllGames()
    {
        var games = await _mediator.Send(new GetAllGamesQuery());
        return Ok(games);
    }

    [HttpPost(nameof(CreateGame))]
    public async Task<ActionResult<GameResponseDTO>> CreateGame(GameRequestDTO gameRequestDTO)
    {
        var game = await _mediator.Send(new CreateGameCommand(gameRequestDTO));
        return Ok(game);
    }

    [HttpPatch(nameof(UpdateGame))]
    public async Task<IActionResult> UpdateGame(GameUpdateDTO gameUpdateDTO)
    {
        await _mediator.Send(new UpdateGameCommand(gameUpdateDTO));
        return Ok();
    }

    [HttpDelete("DeleteGame/{gameId}")]
    public async Task<IActionResult> DeleteGame(string gameId)
    {
        await _mediator.Send(new DeleteGameCommand(gameId));
        return Ok();
    }
}
