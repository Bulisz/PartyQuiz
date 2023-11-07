using Application.DTOs;
using Domain.Games;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.Controllers;

public class AnswerControllerTests : IDisposable
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public AnswerControllerTests()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetAnswersOfQuestionTest()
    {
        //Arrange
        var question = Question.Create(1, 2, "Text1", Guid.Empty).Value;
        var answers = new List<Answer>()
        {
            Answer.Create("Text1", true, Guid.Empty).Value,
            Answer.Create("Text2", false, Guid.Empty).Value
        };
        _factory.QuestionRepository.Get(Arg.Any<Guid>()).Returns(question);
        _factory.AnswerRepository.GetAnswersOfQuestionAsync(Arg.Any<string>()).Returns(answers);

        //Act
        var response = await _client.GetAsync("/partyquiz/answer/GetAnswersOfQuestion/f0e129d4-31a5-40fd-842a-fb1438714977");
        var data = JsonConvert.DeserializeObject<IEnumerable<AnswerResponseDTO>>(await response.Content.ReadAsStringAsync());

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        data.Should().SatisfyRespectively(
            first =>
            {
                first.AnswerText.Should().Be("Text1");
            },
            second =>
            {
                second.IsCorrect.Should().BeFalse();
            });
    }

    [Fact]
    public async Task CreateAnswerTest()
    {
        //Arrange
        var answer = new AnswerRequestDTO("newText", true, Guid.Empty.ToString(), Guid.Empty.ToString());
        var round = Round.Create(1, "Name1", "ABCD", Guid.Empty).Value;
        var question = Question.Create(1, 2, "Text1", Guid.Empty).Value;
        _factory.RoundRepository.Get(Arg.Any<Guid>()).Returns(round);
        _factory.QuestionRepository.Get(Arg.Any<Guid>()).Returns(question);

        //Act
        var response = await _client.PostAsync("/partyquiz/answer/CreateAnswer", JsonContent.Create(answer));
        var data = JsonConvert.DeserializeObject<AnswerResponseDTO>(await response.Content.ReadAsStringAsync());

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        data!.AnswerText.Should().Be("newText");
    }

    [Fact]
    public async Task UpdateanswerTest()
    {
        //Arrange
        var answerToUpdate = new AnswerUpdateDTO(Guid.Empty.ToString(), "NewText");
        var answer = Answer.Create("oldText", true, Guid.Empty).Value;
        _factory.AnswerRepository.Get(Arg.Any<Guid>()).Returns(answer);

        //Act
        var response = await _client.PatchAsync("/partyquiz/answer/UpdateAnswer", JsonContent.Create(answerToUpdate));

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteanswerTest()
    {
        //Arrange
        _factory.AnswerRepository.Get(Arg.Any<Guid>()).Returns(Answer.Create("oldText", true, Guid.Empty).Value);

        //Act
        var response = await _client.DeleteAsync("/partyquiz/answer/DeleteAnswer/f0e129d4-31a5-40fd-842a-fb1438714977");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    public void Dispose()
    {
        _factory.Dispose();
        _client.Dispose();
    }
}