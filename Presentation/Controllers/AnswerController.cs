using Application.DTOs;
using Application.Exceptions;
using Application.Features.Answers.Requests.Commands;
using Application.Features.Answers.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("partyquiz/[controller]")]
public class AnswerController : ControllerBase
{
    private readonly IMediator _mediator;

    public AnswerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetAnswersOfQuestion/{questionId}")]
    public async Task<ActionResult<IEnumerable<AnswerResponseDTO>>> GetAnswersOfQuestion(string questionId)
    {
        var answers = await _mediator.Send(new GetAnswersOfQuestionQuery(questionId));
        return Ok(answers);
    }

    [HttpPost(nameof(CreateAnswer))]
    public async Task<ActionResult<AnswerResponseDTO>> CreateAnswer(AnswerRequestDTO answerRequestDTO)
    {
        var answer = await _mediator.Send(new CreateAnswerCommand(answerRequestDTO));
        return Ok(answer);
    }

    [HttpPatch(nameof(UpdateAnswer))]
    public async Task<IActionResult> UpdateAnswer(AnswerUpdateDTO answerUpdateDTO)
    {
        await _mediator.Send(new UpdateAnswerCommand(answerUpdateDTO));
        return Ok();
    }

    [HttpDelete("DeleteAnswer/{answerId}")]
    public async Task<IActionResult> DeleteAnswer(string answerId)
    {
        await _mediator.Send(new DeleteAnswerCommand(answerId));
        return Ok();
    }
}
