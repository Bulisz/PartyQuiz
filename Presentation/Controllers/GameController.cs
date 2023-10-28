using Application.DTOs;
using Application.Exceptions;
using Application.Features.Games.Requests.Commands;
using Application.Features.Games.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
        try
        {
            var game = await _mediator.Send(new GetGameByNameQuery(gameName));
            return Ok(game);
        }
        catch (QuizValidationException e)
        {
            return BadRequest(new { errors = e.Errors });
        }
    }

    [HttpGet(nameof(GetAllGames))]
    public async Task<ActionResult<IEnumerable<GameResponseDTO>>> GetAllGames()
    {
        var games = await _mediator.Send(new GetAllGameNamesQuery());
        return Ok(games);
    }

    [HttpPost(nameof(CreateGame))]
    public async Task<ActionResult<GameResponseDTO>> CreateGame(GameRequestDTO gameRequestDTO)
    {
        try
        {
            var game = await _mediator.Send(new CreateGameCommand(gameRequestDTO));
            return Ok(game);
        }
        catch (QuizValidationException e)
        {
            return BadRequest(new { errors = e.Errors });
        }
    }

    [HttpPatch(nameof(UpdateGame))]
    public async Task<IActionResult> UpdateGame(GameUpdateDTO gameUpdateDTO)
    {
        try
        {
            await _mediator.Send(new UpdateGameCommand(gameUpdateDTO));
            return Ok();
        }
        catch (QuizValidationException e)
        {
            return BadRequest(new { errors = e.Errors });
        }
    }

    [HttpDelete("DeleteGame/{gameId}")]
    public async Task<IActionResult> DeleteGame(string gameId)
    {
        try
        {
            await _mediator.Send(new DeleteGameCommand(gameId));
            return Ok();
        }
        catch (QuizValidationException e)
        {
            return BadRequest(new { errors = e.Errors});
        }
    }
}
