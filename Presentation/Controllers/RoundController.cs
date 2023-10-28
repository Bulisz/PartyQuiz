using Application.DTOs;
using Application.Exceptions;
using Application.Features.Rounds.Requests.Commands;
using Application.Features.Rounds.Requests.Queries;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("partyquiz/[controller]")]
public class RoundController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoundController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(nameof(GetRoundTypes))]
    public ActionResult<IEnumerable<string>> GetRoundTypes()
    {
        var roundTypes = Enum.GetNames(typeof(RoundType));
        return Ok(roundTypes.ToList());
    }

    [HttpGet("GetRoundsOfGame/{gameId}")]
    public async Task<ActionResult<IEnumerable<GameResponseDTO>>> GetRoundsOfGame(string gameId)
    {
        try
        {
            var rounds = await _mediator.Send(new GetRoundsOfGameQuery(gameId));
            return Ok(rounds);
        }
        catch (QuizValidationException e)
        {
            return BadRequest(new { errors = e.Errors });
        }
    }

    [HttpPost(nameof(CreateRound))]
    public async Task<ActionResult<RoundResponseDTO>> CreateRound(RoundRequestDTO roundRequestDTO)
    {
        try
        {
            var round = await _mediator.Send(new CreateRoundCommand(roundRequestDTO));
            return Ok(round);
        }
        catch (QuizValidationException e)
        {
            return BadRequest(new { errors = e.Errors});
        }
    }

    [HttpPatch(nameof(UpdateRound))]
    public async Task<IActionResult> UpdateRound(RoundUpdateDTO roundUpdateDTO)
    {
        try
        {
            await _mediator.Send(new UpdateRoundCommand(roundUpdateDTO));
            return Ok();
        }
        catch (QuizValidationException e)
        {
            return BadRequest(new { errors = e.Errors });
        }
    }

    [HttpDelete("DeleteRound/{roundId}")]
    public async Task<IActionResult> DeleteRound(string roundId)
    {
        try
        {
            await _mediator.Send(new DeleteRoundCommand(roundId));
            return Ok();
        }
        catch (QuizValidationException e)
        {
            return BadRequest(new { errors = e.Errors });
        }
    }
}
