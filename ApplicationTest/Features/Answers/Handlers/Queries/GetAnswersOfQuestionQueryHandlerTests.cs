using Application.Contracts.Persistence;
using Application.DTOs;
using Application.Exceptions;
using Application.Features.Answers.Handlers.Queries;
using Application.Features.Answers.Requests.Queries;
using Application.Features.Rounds.Requests.Queries;
using Domain.Games;
using FluentAssertions;
using NSubstitute;

namespace ApplicationTest.Features.Answers.Handlers.Queries;

public class GetAnswersOfQuestionQueryHandlerTests
{
    private readonly IAnswerRepository _answerRepository = Substitute.For<IAnswerRepository>();
    private readonly IQuestionRepository _questionRepository = Substitute.For<IQuestionRepository>();
    private readonly GetAnswersOfQuestionQueryHandler _handler;

    public GetAnswersOfQuestionQueryHandlerTests()
    {
        _handler = new GetAnswersOfQuestionQueryHandler(_answerRepository, _questionRepository);
    }

    [Fact]
    public async Task GetRoundsOfGameQueryHandler_ShouldFailure_GameIdIsNotValid()
    {
        //Arrange
        var query = new GetAnswersOfQuestionQuery("34");

        //Act
        var result = () => _handler.Handle(query, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("questionId");
        exception.Which.Errors[0].Message.Should().Be("34 nem valós Id");
    }

    [Fact]
    public async Task GetRoundsOfGameQueryHandler_ShouldFailure_QuestionDoesNotExist()
    {
        //Arrange
        var query = new GetAnswersOfQuestionQuery(Guid.Empty.ToString());
        _questionRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Question?>(null));

        //Act
        var result = () => _handler.Handle(query, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("questionId");
        exception.Which.Errors[0].Message.Should().Be("Question does not exist");
    }

    [Fact]
    public async Task GetRoundsOfGameQueryHandler_ShouldSuccess()
    {
        //Arrange
        var query = new GetAnswersOfQuestionQuery(Guid.Empty.ToString());
        var question = Question.Create(1,3,"QText", Guid.NewGuid()).Value;
        _questionRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Question?>(question));
        var answers = new List<Answer>()
        {
            Answer.Create("AT1", true, Guid.Empty).Value,
            Answer.Create("AT2", false, Guid.Empty).Value
        };
        _answerRepository.GetAnswersOfQuestionAsync(Arg.Any<string>()).Returns(answers);

        //Act
        var result = await _handler.Handle(query, default);

        //Assert
        result.GetType().Should().Be(typeof(List<AnswerResponseDTO>));
        result.Count.Should().Be(2);
        result[0].IsCorrect.Should().Be(true);
        result[1].AnswerText.Should().Be("AT2");
    }
}