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

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            string json = JsonSerializer.Serialize(new { errors = ex.Errors });

            context.Response.ContentType = "application/json; charset=utf-8";
            await context.Response.WriteAsync(json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var problem = new ProblemDetails()
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "Server error",
                Type = "Server error",
                Detail = ex.Message
            };
            string json = JsonSerializer.Serialize(problem);

            context.Response.ContentType = "application/json; charset=utf-8";
            await context.Response.WriteAsync(json);
        }
    }
}
