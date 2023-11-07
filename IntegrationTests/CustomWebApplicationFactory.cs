using Application.Contracts.Persistence;
using Application.Contracts.Persistence.Base;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WebApi;

namespace IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public IGameRepository GameRepository { get; }
    public IRoundRepository RoundRepository { get; }
    public IQuestionRepository QuestionRepository { get; }
    public IAnswerRepository AnswerRepository { get; }

    public CustomWebApplicationFactory()
    {
        GameRepository = Substitute.For<IGameRepository>();
        RoundRepository = Substitute.For<IRoundRepository>();
        QuestionRepository = Substitute.For<IQuestionRepository>();
        AnswerRepository = Substitute.For<IAnswerRepository>();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(services =>
        {
            services.AddSingleton(GameRepository);
            services.AddSingleton(RoundRepository);
            services.AddSingleton(QuestionRepository);
            services.AddSingleton(AnswerRepository);
        });
    }
}