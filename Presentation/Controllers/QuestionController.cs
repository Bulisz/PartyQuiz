﻿using Application.DTOs;
using Application.Exceptions;
using Application.Features.Questions.Requests.Commands;
using Application.Features.Questions.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("partyquiz/[controller]")]
public class QuestionController : ControllerBase
{
    private readonly IMediator _mediator;

    public QuestionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetQuestionsOfRound/{roundId}")]
    public async Task<ActionResult<IEnumerable<QuestionResponseDTO>>> GetQuestionsOfRound(string roundId)
    {
        var questions = await _mediator.Send(new GetQuestionsOfRoundQuery(roundId));
        return Ok(questions);
    }

    [HttpPost(nameof(CreateQuestion))]
    public async Task<ActionResult<QuestionResponseDTO>> CreateQuestion(QuestionRequestDTO questionRequestDTO)
    {
        try
        {
            var question = await _mediator.Send(new CreateQuestionCommand(questionRequestDTO));
            return Ok(question);
        }
        catch (QuizValidationException e)
        {
            return BadRequest(new { errors = e.Errors });
        }
    }

    [HttpPatch(nameof(UpdateQuestion))]
    public async Task<IActionResult> UpdateQuestion(QuestionUpdateDTO questionUpdateDTO)
    {
        try
        { 
            await _mediator.Send(new UpdateQuestionCommand(questionUpdateDTO));
            return Ok();
        }
        catch (QuizValidationException e)
        {
            return BadRequest(new { errors = e.Errors });
        }
    }

    [HttpDelete("DeleteQuestion/{questionId}")]
    public async Task<IActionResult> DeleteQuestion(string questionId)
    {
        try
        {
            await _mediator.Send(new DeleteQuestionCommand(questionId));
            return Ok();
        }
        catch (QuizValidationException e)
        {
            return BadRequest(new { errors = e.Errors });
        }
    }
}
