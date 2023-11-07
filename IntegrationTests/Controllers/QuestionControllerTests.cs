using Application.DTOs;
using Domain.Games;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.Controllers;

public class QuestionControllerTests : IDisposable
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public QuestionControllerTests()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetQuestionsOfRoundTest()
    {
        //Arrange
        var round = Round.Create(1, "Name1", "ABCD", Guid.Empty).Value;
        var questions = new List<Question>()
        {
            Question.Create(1, 2, "Text1", Guid.Empty).Value,
            Question.Create(2, 3, "Text2", Guid.Empty).Value
        };
        _factory.RoundRepository.Get(Arg.Any<Guid>()).Returns(round);
        _factory.QuestionRepository.GetQuestionsOfRoundAsync(Arg.Any<string>()).Returns(questions);

        //Act
        var response = await _client.GetAsync("/partyquiz/question/GetQuestionsOfRound/f0e129d4-31a5-40fd-842a-fb1438714977");
        var data = JsonConvert.DeserializeObject<IEnumerable<QuestionResponseDTO>>(await response.Content.ReadAsStringAsync());

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        data.Should().SatisfyRespectively(
            first =>
            {
                first.QuestionText.Should().Be("Text1");
            },
            second =>
            {
                second.FullScore.Should().Be(3);
            });
    }

    [Fact]
    public async Task CreateQuestionTest()
    {
        //Arrange
        var question = new QuestionRequestDTO(5, "newText", Guid.Empty.ToString());
        var round = Round.Create(1, "Name1", "ABCD", Guid.Empty).Value;
        var questions = new List<Question>()
        {
            Question.Create(1, 2, "Text1", Guid.Empty).Value,
            Question.Create(2, 3, "Text2", Guid.Empty).Value
        };
        _factory.RoundRepository.Get(Arg.Any<Guid>()).Returns(round);
        _factory.QuestionRepository.GetQuestionsOfRoundAsync(Arg.Any<string>()).Returns(questions);

        //Act
        var response = await _client.PostAsync("/partyquiz/question/CreateQuestion", JsonContent.Create(question));
        var data = JsonConvert.DeserializeObject<QuestionResponseDTO>(await response.Content.ReadAsStringAsync());

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        data!.QuestionNumber.Should().Be(3);
    }

    [Fact]
    public async Task UpdateQuestionTest()
    {
        //Arrange
        var questionToUpdate = new QuestionUpdateDTO(Guid.Empty.ToString(), 1, "NewText");
        var question = Question.Create(1, 2, "oldText", Guid.Empty).Value;
        _factory.QuestionRepository.Get(Arg.Any<Guid>()).Returns(question);

        //Act
        var response = await _client.PatchAsync("/partyquiz/question/UpdateQuestion", JsonContent.Create(questionToUpdate));

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteRoundTest()
    {
        //Arrange
        _factory.QuestionRepository.Get(Arg.Any<Guid>()).Returns(Question.Create(1, 2, "oldText", Guid.Empty).Value);

        //Act
        var response = await _client.DeleteAsync("/partyquiz/question/DeleteQuestion/f0e129d4-31a5-40fd-842a-fb1438714977");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    public void Dispose()
    {
        _factory.Dispose();
        _client.Dispose();
    }
}