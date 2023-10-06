using Application;
using Infrastructure;
using Persistence;
using Presentation;
using Serilog;

namespace WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddApplication()
            .AddInfrastructure()
            .AddPersistence()
            .AddPresentation();

        builder.Host.UseSerilog((context, conig) =>
            conig.ReadFrom.Configuration(context.Configuration));

        builder.Services.AddControllers()
            .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);

        var app = builder.Build();

        app.UseSerilogRequestLogging();

        app.UseHttpsRedirection();

        app.Run();
    }
}