using Application.Contracts.Persistence;
using Application.DTOs;
using Application.Exceptions;
using Application.Features.Questions.Handlers.Queries;
using Application.Features.Questions.Requests.Queries;
using Application.Features.Rounds.Requests.Queries;
using Domain.Games;
using FluentAssertions;
using NSubstitute;

namespace ApplicationTest.Features.Questions.Queries;

public class GetQuestionsOfRoundQueryHandlerTest
{
    private readonly IQuestionRepository _questionRepository = Substitute.For<IQuestionRepository>();
    private readonly IRoundRepository _roundRepository = Substitute.For<IRoundRepository>();
    private readonly GetQuestionsOfRoundQueryHandler _handler;

    public GetQuestionsOfRoundQueryHandlerTest()
    {
        _handler = new GetQuestionsOfRoundQueryHandler(_questionRepository, _roundRepository);
    }

    [Fact]
    public async Task GetQuestionsOfRoundQueryHandler_ShouldFailure_RoundIdIsNotValid()
    {
        //Arrange
        var query = new GetQuestionsOfRoundQuery("34");

        //Act
        var result = () => _handler.Handle(query, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("roundId");
        exception.Which.Errors[0].Message.Should().Be("34 nem valós Id");
    }

    [Fact]
    public async Task GetQuestionsOfRoundQueryHandler_ShouldFailure_GameDoesNotExist()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var query = new GetQuestionsOfRoundQuery(guid.ToString());
        _roundRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Round?>(null));

        //Act
        var result = () => _handler.Handle(query, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("roundId");
        exception.Which.Errors[0].Message.Should().Be("Round does not exist");
    }

    [Fact]
    public async Task GetQuestionsOfRoundQueryHandler_ShouldFailure_ShouldSuccess()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var query = new GetQuestionsOfRoundQuery(guid.ToString());
        var round = Round.Create(1, "RoundName", "ABCD", Guid.Empty).Value;
        _roundRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Round?>(round));
        var questionList = new List<Question>()
            {
                Question.Create(1,1,"Text1", Guid.Empty).Value,
                Question.Create(2,2,"Text2", Guid.Empty).Value
            };
        _questionRepository.GetQuestionsOfRoundAsync(guid.ToString()).Returns(questionList);

        //Act
        var result = await _handler.Handle(query, default);

        //Assert
        result.GetType().Should().Be(typeof(List<QuestionResponseDTO>));
        result[0].QuestionText.Should().Be("Text1");
        result[1].FullScore.Should().Be(2);
    }
}