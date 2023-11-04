using Microsoft.EntityFrameworkCore;
using Persistence;

namespace PersistenceTest;

public static class ContextGenerator
{
    public static PartyQuizDbContext Generate()
    {
        var optionsBuilder = new DbContextOptionsBuilder<PartyQuizDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        return new PartyQuizDbContext(optionsBuilder.Options);
    }
}