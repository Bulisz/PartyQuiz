using Application.Contracts.Persistence;
using Application.Contracts.Persistence.Base;
using Application.DTOs;
using Application.Exceptions;
using Application.Features.Questions.Handlers.Commands;
using Application.Features.Questions.Requests.Commands;
using Domain.Games;
using FluentAssertions;
using NSubstitute;

namespace ApplicationTest.Features.Questions.Handlers.Commands;

public class CreateQuestionCommandHandlerTests
{
    private readonly IQuestionRepository _questionRepository = Substitute.For<IQuestionRepository>();
    private readonly IRoundRepository _roundRepository = Substitute.For<IRoundRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly CreateQuestionCommandHandler _handler;

    public CreateQuestionCommandHandlerTests()
    {
        _handler = new CreateQuestionCommandHandler(_unitOfWork, _questionRepository, _roundRepository);
    }

    [Fact]
    public async Task CreateQuestionCommandHandlerTests_ShouldFailure_AllInputAreInvalid()
    {
        //Arrange
        var dto = new QuestionRequestDTO(11, "", "34");
        var command = new CreateQuestionCommand(dto);

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(3);
        exception.Which.Errors[0].Field.Should().Be("roundId");
        exception.Which.Errors[0].Message.Should().Be("34 nem valós Id");
        exception.Which.Errors[1].Field.Should().Be("fullScore");
        exception.Which.Errors[2].Field.Should().Be("questionText");
    }

    [Fact]
    public async Task CreateQuestionCommandHandlerTests_ShouldFailure_RoundDoesNotExist()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var dto = new QuestionRequestDTO(5, "Text", guid.ToString());
        var command = new CreateQuestionCommand(dto);
        _roundRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Round?>(null));

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("roundId");
        exception.Which.Errors[0].Message.Should().Be("Round does not exist");
    }

    [Fact]
    public async Task CreateQuestionCommandHandlerTests_ShouldSuccess()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var dto = new QuestionRequestDTO(5, "Text", guid.ToString());
        var command = new CreateQuestionCommand(dto);
        var round = Round.Create(1, "RoundName", "ABCD", Guid.Empty).Value;
        _roundRepository.Get(Arg.Any<Guid>())!.Returns(Task.FromResult(round));
        _questionRepository.GetQuestionsOfRoundAsync(Arg.Any<string>()).Returns(Task.FromResult(new List<Question>()));

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        result.GetType().Should().Be(typeof(QuestionResponseDTO));
        result.QuestionNumber.Should().Be(1);
        result.QuestionText.Should().Be("Text");
        result.FullScore.Should().Be(5);
    }

    [Fact]
    public async Task CreateQuestionCommandHandlerTests_ShouldCall_RepositoryMethodsProperly()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var dto = new QuestionRequestDTO(5, "Text", guid.ToString());
        var command = new CreateQuestionCommand(dto);
        var round = Round.Create(1, "RoundName", "ABCD", Guid.Empty).Value;
        _roundRepository.Get(Arg.Any<Guid>())!.Returns(Task.FromResult(round));
        _questionRepository.GetQuestionsOfRoundAsync(Arg.Any<string>()).Returns(Task.FromResult(new List<Question>()));

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        await _roundRepository.Received(1).Get(guid);
        await _questionRepository.Received(1).GetQuestionsOfRoundAsync(guid.ToString());
        await _questionRepository.Received(1).Add(Arg.Any<Question>());
        await _unitOfWork.Received(1).Save();
    }

    [Fact]
    public async Task CreateQuestionCommandHandlerTests_ShouldNotCall_RepositoryMethodsProperly()
    {
        //Arrange
        var dto = new QuestionRequestDTO(11, "", "34");
        var command = new CreateQuestionCommand(dto);

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        await _roundRepository.DidNotReceiveWithAnyArgs().Get(Arg.Any<Guid>());
        await _questionRepository.DidNotReceiveWithAnyArgs().GetQuestionsOfRoundAsync(Arg.Any<string>());
        await _questionRepository.DidNotReceiveWithAnyArgs().Add(Arg.Any<Question>());
        await _unitOfWork.DidNotReceiveWithAnyArgs().Save();

    }
}