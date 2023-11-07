using Application.DTOs;
using Domain.Games;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.Controllers;

public class GameControllerTests : IDisposable
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public GameControllerTests()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetGameByNameTest()
    {
        //Arrange
        var game = Game.Create("GameName").Value;
        _factory.GameRepository.GetGameByNameAsync(Arg.Any<string>()).Returns(Task.FromResult<Game?>(game));

        //Act
        var response = await _client.GetAsync("/partyquiz/game/GetGameByName/GameName");
        var data = JsonConvert.DeserializeObject<GameResponseDTO>(await response.Content.ReadAsStringAsync());

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        data!.GameName.Should().Be("GameName");
    }

    [Fact]
    public async Task GetAllGamesTest()
    {
        //Arrange
        var games = new List<Game>()
        {
            Game.Create("Game1").Value,
            Game.Create("Game2").Value
        };
        _factory.GameRepository.GetAllGameNames().Returns(games);

        //Act
        var response = await _client.GetAsync("/partyquiz/game/GetAllGames");
        var data = JsonConvert.DeserializeObject<IEnumerable<GameResponseDTO>>(await response.Content.ReadAsStringAsync());

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        data.Should().SatisfyRespectively(
            first =>
            {
                first.GameName.Should().Be("Game1");
            },
            second =>
            {
                second.GameName.Should().Be("Game2");
            });
    }

    [Fact]
    public async Task CreateGameTest()
    {
        //Arrange
        var gameToCreate = new GameRequestDTO("GameName");
        _factory.GameRepository.Add(Arg.Any<Game>()).Returns(c => Game.Create((c[0] as Game)!.GameName).Value);

        //Act
        var response = await _client.PostAsync("/partyquiz/game/CreateGame", JsonContent.Create(gameToCreate));
        var data = JsonConvert.DeserializeObject<GameResponseDTO>(await response.Content.ReadAsStringAsync());

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        data!.GameName.Should().Be("GameName");
    }

    [Fact]
    public async Task UpdateGameTest()
    {
        //Arrange
        var gameToUpdate = new GameUpdateDTO(Guid.Empty.ToString(), "NewName");
        _factory.GameRepository.Get(Arg.Any<Guid>()).Returns(Game.Create("OldName").Value);

        //Act
        var response = await _client.PatchAsync("/partyquiz/game/UpdateGame", JsonContent.Create(gameToUpdate));

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteGameTest()
    {
        //Arrange
        _factory.GameRepository.Get(Arg.Any<Guid>()).Returns(Game.Create("GameName").Value);

        //Act
        var response = await _client.DeleteAsync("/partyquiz/game/DeleteGame/f0e129d4-31a5-40fd-842a-fb1438714977");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    public void Dispose()
    {
        _factory.Dispose();
        _client.Dispose();
    }
}