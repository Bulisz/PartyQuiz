using Persistence.Repositories;
using Persistence;
using Domain.Games;
using FluentAssertions;

namespace PersistenceTest.Repositories;

public class QuestionRepositoryTests
{
    private readonly PartyQuizDbContext _context;
    private readonly QuestionRepository _repository;
    public QuestionRepositoryTests()
    {
        _context = ContextGenerator.Generate();
        _repository = new QuestionRepository(_context);
    }

    [Fact]
    public async Task Get_Test()
    {
        //Arrange
        var game = Game.Create("NewGame").Value;
        _context.Games.Add(game);
        await _context.SaveChangesAsync();
        var round = Round.Create(1, "RoundName", "ABCD", game.Id).Value;
        game.TryToAddRound(round);
        _context.Games.Update(game);
        await _context.SaveChangesAsync();
        var question = Question.Create(1, 1, "QestionText", round.Id).Value;
        _context.Questions.Add(question);
        await _context.SaveChangesAsync();
        var answer = Answer.Create("AnswerText", true, question.Id).Value;
        var answer2 = Answer.Create("AT2", false, question.Id).Value;
        _context.Answers.Add(answer);
        _context.Answers.Add(answer2);
        await _context.SaveChangesAsync();


        //Act
        var result = await _repository.Get(question.Id);

        //Assert
        result!.FullScore.Should().Be(1);
        result.Answers.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetQuestionsOfRoundAsync_Test()
    {
        //Arrange
        var game = Game.Create("NewGame").Value;
        _context.Games.Add(game);
        await _context.SaveChangesAsync();
        var round = Round.Create(1, "RoundName", "ABCD", game.Id).Value;
        game.TryToAddRound(round);
        _context.Games.Update(game);
        await _context.SaveChangesAsync();
        var question = Question.Create(1, 1, "QestionText", round.Id).Value;
        var question2 = Question.Create(2,2,"QT2",round.Id).Value;
        _context.Questions.Add(question);
        _context.Questions.Add(question2);
        await _context.SaveChangesAsync();
        var answer = Answer.Create("AnswerText", true, question.Id).Value;
        var answer2 = Answer.Create("AT2", false, question.Id).Value;
        _context.Answers.Add(answer);
        _context.Answers.Add(answer2);
        await _context.SaveChangesAsync();

        //Act
        var result = await _repository.GetQuestionsOfRoundAsync(round.Id.ToString());

        //Assert
        result.Should().HaveCount(2);
        result[0].Answers.Should().HaveCount(2);
    }
}