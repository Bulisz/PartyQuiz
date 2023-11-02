using Application.DTOs;
using Application.Features.Games.Requests.Commands;
using Application.Features.Games.Requests.Queries;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Presentation.Controllers;

namespace PresentationTest.Controllers;

public class GameControllerTests
{
    public readonly IMediator _mediator = Substitute.For<IMediator>();
    public readonly GameController _controller;

    public GameControllerTests()
    {
        _controller = new GameController(_mediator);
    }

    [Fact]
    public async Task GetGameByNameTest_ShouldSuccess()
    {
        //Arrange
        _mediator.Send(Arg.Any<GetGameByNameQuery>()).Returns(c => new GameResponseDTO("0001", (c[0] as GetGameByNameQuery)!.GameName, new()));

        //Act
        var result = await _controller.GetGameByName("MyGame");
        var value = (result.Result as OkObjectResult)!.Value as GameResponseDTO;

        //Assert
        Assert.IsType<ActionResult<GameResponseDTO>>(result);
        value!.GameName.Should().Be("MyGame");
        value!.Id.Should().Be("0001");
        value!.Rounds.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllGamesTest_SouldSuccess()
    {
        //Arrange
        _mediator.Send(Arg.Any<GetAllGameNamesQuery>()).Returns(new List<GameResponseDTO>()
        {
            new GameResponseDTO("0001", "Name1", new()),
            new GameResponseDTO("0002", "Name2", new())
        });

        //Act
        var result = await _controller.GetAllGames();
        var value = (result.Result as OkObjectResult)!.Value as List<GameResponseDTO>;

        //Assert
        Assert.IsType<ActionResult<IEnumerable<GameResponseDTO>>>(result);
        value!.Count.Should().Be(2);
        value![0].Id.Should().Be("0001");
        value![1].GameName.Should().Be("Name2");
    }

    [Fact]
    public async Task CreateGameTest_ShouldSuccess()
    {
        //Arrange
        var dto = new GameRequestDTO("Test1");
        _mediator.Send(Arg.Any<CreateGameCommand>()).Returns(c => new GameResponseDTO("0001", (c[0] as CreateGameCommand)!.GameRequestDTO.GameName, new()));

        //Act
        var result = await _controller.CreateGame(dto);
        var value = (result.Result as OkObjectResult)!.Value as GameResponseDTO;

        //Assert
        Assert.IsType<ActionResult<GameResponseDTO>>(result);
        value!.GameName.Should().Be("Test1");
        value!.Id.Should().Be("0001");
        value!.Rounds.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateGameTest_ShouldSuccess()
    {
        //Arrange
        var dto = new GameUpdateDTO("0001", "Name1");
        _ = _mediator.Send(Arg.Any<UpdateGameCommand>());

        //Act
        var result = await _controller.UpdateGame(dto);

        //Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteGameTest_ShouldSuccess()
    {
        //Arrange
        _ = _mediator.Send(Arg.Any<DeleteGameCommand>());

        //Act
        var result = await _controller.DeleteGame("Test");

        //Assert
        Assert.IsType<OkResult>(result);
    }
}