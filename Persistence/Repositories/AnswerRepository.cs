using Application.Contracts.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Base;

namespace Persistence.Repositories;

public class AnswerRepository : GenericRepository<Answer>, IAnswerRepository
{
    private readonly PartyQuizDbContext _context;

    public AnswerRepository(PartyQuizDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Answer>> GetAnswersOfQuestionAsync(string questionId)
    {
        return await _context.Answers.Where(a => a.QuestionId.ToString() == questionId).ToListAsync();
    }
}
