using Application.DTOs;
using Application.Features.Answers.Requests.Commands;
using Application.Features.Answers.Requests.Queries;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Presentation.Controllers;

namespace PresentationTest.Controllers;

public class AnswerControllerTests
{
    public readonly IMediator _mediator = Substitute.For<IMediator>();
    public readonly AnswerController _controller;

    public AnswerControllerTests()
    {
        _controller = new AnswerController(_mediator);
    }

    [Fact]
    public async Task GetAnswersOfRoundTest_ShouldSuccess()
    {
        //Arrange
        _mediator.Send(Arg.Any<GetAnswersOfQuestionQuery>()).Returns(c => new List<AnswerResponseDTO>()
        {
            new AnswerResponseDTO("0001", "Text1", true),
            new AnswerResponseDTO("0002", "Text2", false)
        });

        //Act
        var result = await _controller.GetAnswersOfQuestion("0000");
        var value = (result.Result as OkObjectResult)!.Value as List<AnswerResponseDTO>;

        //Assert
        Assert.IsType<ActionResult<IEnumerable<AnswerResponseDTO>>>(result);
        value!.Count().Should().Be(2);
        value![1].AnswerText.Should().Be("Text2");
    }

    [Fact]
    public async Task CreateAnswerTest_ShouldSuccess()
    {
        //Arrange
        var dto = new AnswerRequestDTO("Text1", true, "0003", "0004");
        _mediator.Send(Arg.Any<CreateAnswerCommand>()).Returns(c => new AnswerResponseDTO(
            "0001",
            (c[0] as CreateAnswerCommand)!.AnswerRequestDTO.AnswerText,
            (c[0] as CreateAnswerCommand)!.AnswerRequestDTO.IsCorrect));

        //Act
        var result = await _controller.CreateAnswer(dto);
        var value = (result.Result as OkObjectResult)!.Value as AnswerResponseDTO;

        //Assert
        Assert.IsType<ActionResult<AnswerResponseDTO>>(result);
        value!.Id.Should().Be("0001");
        value!.AnswerText.Should().Be("Text1");
        value!.IsCorrect.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAnswerTest_ShouldSuccess()
    {
        //Arrange
        var dto = new AnswerUpdateDTO("0001", "Text1");
        _ = _mediator.Send(Arg.Any<UpdateAnswerCommand>());

        //Act
        var result = await _controller.UpdateAnswer(dto);

        //Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteAnswerTest_ShouldSuccess()
    {
        //Arrange
        _ = _mediator.Send(Arg.Any<DeleteAnswerCommand>());

        //Act
        var result = await _controller.DeleteAnswer("Test");

        //Assert
        Assert.IsType<OkResult>(result);
    }
}