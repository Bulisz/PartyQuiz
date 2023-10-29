using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace WebApi.Middlewares;

public class GlobalExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch(QuizValidationException ex)
        {
            _logger.LogError(ex, ex.Message);

            string json = JsonSerializer.Serialize(new { errors = ex.Errors });

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            var problem = new ProblemDetails()
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "Server error",
                Type = "Server error",
                Detail = ex.Message
            };

            await RespondError(context, problem);
        }
    }

    private static async Task RespondError(HttpContext context, ProblemDetails problem)
    {
        context.Response.StatusCode = problem.Status ?? (int)HttpStatusCode.InternalServerError;

        string json = JsonSerializer.Serialize(problem);

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(json);
    }
}
