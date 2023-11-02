using Application.DTOs;
using Application.Features.Games.Requests.Commands;
using Application.Features.Rounds.Requests.Commands;
using Application.Features.Rounds.Requests.Queries;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Presentation.Controllers;

namespace PresentationTest.Controllers;

public class RoundControllerTests
{
    public readonly IMediator _mediator = Substitute.For<IMediator>();
    public readonly RoundController _controller;

    public RoundControllerTests()
    {
        _controller = new RoundController(_mediator);
    }

    [Fact]
    public void GetRoundTypesTest_ShouldSucces()
    {
        //Act
        var result = _controller.GetRoundTypes();
        var value =(result.Result as OkObjectResult)!.Value as List<string>;

        //Assert
        Assert.IsType<ActionResult<IEnumerable<string>>>(result);
        value!.Count().Should().Be(6);
        value.Should().Contain("ABCD");
    }

    [Fact]
    public async Task GetRoundsOfGameTest_ShouldSuccess()
    {
        //Arrange
        _mediator.Send(Arg.Any<GetRoundsOfGameQuery>()).Returns(c => new List<RoundResponseDTO>()
        {
            new RoundResponseDTO("0001", 1, "Name1", "ABCD", "0000", new()),
            new RoundResponseDTO("0002", 1, "Name2", "Nullable", "0000", new())
        });

        //Act
        var result = await _controller.GetRoundsOfGame("0000");
        var value = (result.Result as OkObjectResult)!.Value as List<RoundResponseDTO>;

        //Assert
        Assert.IsType<ActionResult<IEnumerable<RoundResponseDTO>>>(result);
        value!.Count().Should().Be(2);
        value![1].RoundName.Should().Be("Name2");
    }

    [Fact]
    public async Task CreateRoundTest_ShouldSuccess()
    {
        //Arrange
        var dto = new RoundRequestDTO(1, "Test1", "ABCD", "0000");
        _mediator.Send(Arg.Any<CreateRoundCommand>()).Returns(c => new RoundResponseDTO(
            "0001",
            (c[0] as CreateRoundCommand)!.RoundRequestDTO.RoundNumber,
            (c[0] as CreateRoundCommand)!.RoundRequestDTO.RoundName,
            (c[0] as CreateRoundCommand)!.RoundRequestDTO.RoundType,
            (c[0] as CreateRoundCommand)!.RoundRequestDTO.GameId,
            new()));

        //Act
        var result = await _controller.CreateRound(dto);
        var value = (result.Result as OkObjectResult)!.Value as RoundResponseDTO;

        //Assert
        Assert.IsType<ActionResult<RoundResponseDTO>>(result);
        value!.RoundName.Should().Be("Test1");
        value!.Id.Should().Be("0001");
        value!.RoundType.Should().Be("ABCD");
        value!.Questions.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateRoundTest_ShouldSuccess()
    {
        //Arrange
        var dto = new RoundUpdateDTO("0001", 1, "Test1", "ABCD", "0000");
        _ = _mediator.Send(Arg.Any<UpdateRoundCommand>());

        //Act
        var result = await _controller.UpdateRound(dto);

        //Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteRoundTest_ShouldSuccess()
    {
        //Arrange
        _ = _mediator.Send(Arg.Any<DeleteRoundCommand>());

        //Act
        var result = await _controller.DeleteRound("Test");

        //Assert
        Assert.IsType<OkResult>(result);
    }
}