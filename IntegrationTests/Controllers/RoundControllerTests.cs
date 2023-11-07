using Application.DTOs;
using Domain.Games;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.Controllers;

public class RoundControllerTests : IDisposable
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public RoundControllerTests()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetRoundTypesTest()
    {
        //Act
        var response = await _client.GetAsync("/partyquiz/round/GetRoundTypes");
        var data = JsonConvert.DeserializeObject<IEnumerable<string>>(await response.Content.ReadAsStringAsync());

        //Assert
        data.Should().HaveCount(6);
        data.Should().Contain("ABCD");
    }

    [Fact]
    public async Task GetRoundsOfGameTest()
    {
        //Arrange
        var game = Game.Create("GameName").Value;
        var rounds = new List<Round>()
        {
            Round.Create(1, "Name1", "ABCD", Guid.Empty).Value,
            Round.Create(2, "Name2" ,"Nullable", Guid.Empty).Value
        };
        _factory.GameRepository.Get(Arg.Any<Guid>()).Returns(game);
        _factory.RoundRepository.GetRoundsOfGameAsync(Arg.Any<string>()).Returns(rounds);

        //Act
        var response = await _client.GetAsync("/partyquiz/round/GetRoundsOfGame/f0e129d4-31a5-40fd-842a-fb1438714977");
        var data = JsonConvert.DeserializeObject<IEnumerable<RoundResponseDTO>>(await response.Content.ReadAsStringAsync());

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        data.Should().SatisfyRespectively(
            first =>
            {
                first.RoundName.Should().Be("Name1");
            },
            second =>
            {
                second.RoundName.Should().Be("Name2");
            });
    }

    [Fact]
    public async Task CreateRoundTest()
    {
        //Arrange
        var round = new RoundRequestDTO(1, "Name", "ABCD", Guid.Empty.ToString());
        var game = Game.Create("GameName").Value;
        _factory.GameRepository.Get(Arg.Any<Guid>()).Returns(game);

        //Act
        var response = await _client.PostAsync("/partyquiz/round/CreateRound", JsonContent.Create(round));
        var data = JsonConvert.DeserializeObject<RoundResponseDTO>(await response.Content.ReadAsStringAsync());

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        data!.RoundName.Should().Be("Name");
    }

    [Fact]
    public async Task UpdateRoundTest()
    {
        //Arrange
        var roundToUpdate = new RoundUpdateDTO(Guid.Empty.ToString(), 1, "NewName", "ABCD", Guid.Empty.ToString());
        var game = Game.Create("OldName").Value;
        game.TryToAddRound(Round.Create(1, "OldName", "ABCD", Guid.Empty).Value);
        _factory.GameRepository.Get(Arg.Any<Guid>()).Returns(game);

        //Act
        var response = await _client.PatchAsync("/partyquiz/round/UpdateRound", JsonContent.Create(roundToUpdate));

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteRoundTest()
    {
        //Arrange
        _factory.RoundRepository.Get(Arg.Any<Guid>()).Returns(Round.Create(1, "OldName", "ABCD", Guid.Empty).Value);

        //Act
        var response = await _client.DeleteAsync("/partyquiz/round/DeleteRound/f0e129d4-31a5-40fd-842a-fb1438714977");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    public void Dispose()
    {
        _factory.Dispose();
        _client.Dispose();
    }
}