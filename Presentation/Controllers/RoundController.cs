﻿using Application.DTOs;
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
    public async Task<ActionResult<IEnumerable<RoundResponseDTO>>> GetRoundsOfGame(string gameId)
    {
        var rounds = await _mediator.Send(new GetRoundsOfGameQuery(gameId));
        return Ok(rounds);
    }

    [HttpPost(nameof(CreateRound))]
    public async Task<ActionResult<RoundResponseDTO>> CreateRound(RoundRequestDTO roundRequestDTO)
    {
        var round = await _mediator.Send(new CreateRoundCommand(roundRequestDTO));
        return Ok(round);
    }

    [HttpPatch(nameof(UpdateRound))]
    public async Task<IActionResult> UpdateRound(RoundUpdateDTO roundUpdateDTO)
    {
        await _mediator.Send(new UpdateRoundCommand(roundUpdateDTO));
        return Ok();
    }

    [HttpDelete("DeleteRound/{roundId}")]
    public async Task<IActionResult> DeleteRound(string roundId)
    {
        await _mediator.Send(new DeleteRoundCommand(roundId));
        return Ok();
    }
}
