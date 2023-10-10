using Application;
using Infrastructure;
using Persistence;
using Presentation;
using Presentation.Controllers;
using Serilog;
using System.Reflection;

namespace WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddApplication()
            .AddInfrastructure()
            .AddPersistence(builder.Configuration)
            .AddPresentation();

        builder.Host.UseSerilog((context, config) =>
            config.ReadFrom.Configuration(context.Configuration));

        builder.Services.AddControllers();

        var app = builder.Build();

        app.UseSerilogRequestLogging();

        app.UseHttpsRedirection();

        app.MapControllers();

        app.Run();
    }
}