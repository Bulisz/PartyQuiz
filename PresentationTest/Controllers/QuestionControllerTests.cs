using Application.DTOs;
using Application.Features.Questions.Requests.Commands;
using Application.Features.Questions.Requests.Queries;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Presentation.Controllers;

namespace PresentationTest.Controllers;

public class QuestionControllerTests
{
    public readonly IMediator _mediator = Substitute.For<IMediator>();
    public readonly QuestionController _controller;

    public QuestionControllerTests()
    {
        _controller = new QuestionController(_mediator);
    }

    [Fact]
    public async Task GetQuestionsOfRoundTest_ShouldSuccess()
    {
        //Arrange
        _mediator.Send(Arg.Any<GetQuestionsOfRoundQuery>()).Returns(c => new List<QuestionResponseDTO>()
        {
            new QuestionResponseDTO("0001", 1, 3, "Text1", "0000", new()),
            new QuestionResponseDTO("0002", 1, 5, "Text2", "0000", new())
        });

        //Act
        var result = await _controller.GetQuestionsOfRound("0000");
        var value = (result.Result as OkObjectResult)!.Value as List<QuestionResponseDTO>;

        //Assert
        Assert.IsType<ActionResult<IEnumerable<QuestionResponseDTO>>>(result);
        value!.Count().Should().Be(2);
        value![1].QuestionText.Should().Be("Text2");
    }

    [Fact]
    public async Task CreateQuestionTest_ShouldSuccess()
    {
        //Arrange
        var dto = new QuestionRequestDTO(1, "Text1", "0000");
        _mediator.Send(Arg.Any<CreateQuestionCommand>()).Returns(c => new QuestionResponseDTO(
            "0001",
            1,
            (c[0] as CreateQuestionCommand)!.QuestionRequestDTO.FullScore,
            (c[0] as CreateQuestionCommand)!.QuestionRequestDTO.QuestionText,
            (c[0] as CreateQuestionCommand)!.QuestionRequestDTO.RoundId,
            new()));

        //Act
        var result = await _controller.CreateQuestion(dto);
        var value = (result.Result as OkObjectResult)!.Value as QuestionResponseDTO;

        //Assert
        Assert.IsType<ActionResult<QuestionResponseDTO>>(result);
        value!.FullScore.Should().Be(1);
        value!.Id.Should().Be("0001");
        value!.QuestionText.Should().Be("Text1");
        value!.Answers.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateQuestionTest_ShouldSuccess()
    {
        //Arrange
        var dto = new QuestionUpdateDTO("0001", 1, "Text1");
        _ = _mediator.Send(Arg.Any<UpdateQuestionCommand>());

        //Act
        var result = await _controller.UpdateQuestion(dto);

        //Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteQuestionTest_ShouldSuccess()
    {
        //Arrange
        _ = _mediator.Send(Arg.Any<DeleteQuestionCommand>());

        //Act
        var result = await _controller.DeleteQuestion("Test");

        //Assert
        Assert.IsType<OkResult>(result);
    }
}