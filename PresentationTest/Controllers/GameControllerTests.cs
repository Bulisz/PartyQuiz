using Application.DTOs;
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

    [Fact]
    public async Task GetGameByName_ShouldSuccess()
    {
        //Arrange
        _mediator.Send(Arg.Any<GetGameByNameQuery>()).Returns(c => new GameResponseDTO("0001", (c[0] as GetGameByNameQuery)!.GameName, new()));
        var controller = new GameController(_mediator);

        //Act
        var result = await controller.GetGameByName("MyGame");
        var value = (result.Result as OkObjectResult)!.Value as GameResponseDTO;

        //Assert
        Assert.IsType<ActionResult<GameResponseDTO>>(result);
        value!.GameName.Should().Be("MyGame");
        value!.Id.Should().Be("0001");
        value!.Rounds.Should().BeEmpty();
    }
}