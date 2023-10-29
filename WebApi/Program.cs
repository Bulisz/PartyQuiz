using Application;
using Infrastructure;
using Persistence;
using Presentation;
using Serilog;
using WebApi.Middlewares;

namespace WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        {
            builder.Services
                .AddApplication()
                .AddInfrastructure()
                .AddPersistence(builder.Configuration)
                .AddPresentation();

            builder.Host.UseSerilog((context, config) =>
                config.ReadFrom.Configuration(context.Configuration));

            builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

            builder.Services.AddControllers();
        }

        var app = builder.Build();
        {
            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}