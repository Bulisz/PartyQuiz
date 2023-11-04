using Domain.Astractions;
using Domain.Games;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class PartyQuizDbContext : DbContext
{
    public DbSet<Game> Games { get; set; }
    public DbSet<Round> Rounds { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }

    public PartyQuizDbContext(DbContextOptions<PartyQuizDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<DomainEvent>();
    }
}
