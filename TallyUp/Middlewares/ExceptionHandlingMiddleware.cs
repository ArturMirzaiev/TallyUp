using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation;
using TallyUp.Domain.Responses;

namespace TallyUp.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var errorResponse = ex switch
            {
                ValidationException validationEx => new ErrorResponse(
                    StatusCodes.Status400BadRequest, 
                    description: "Validation error: " + string.Join(", ", validationEx.Errors.Select(e => e.ErrorMessage))),
                KeyNotFoundException => new ErrorResponse(StatusCodes.Status404NotFound, description: "Resource not found"),
                UnauthorizedAccessException => new ErrorResponse(StatusCodes.Status401Unauthorized, description: "Unauthorized access"),
                ArgumentException argEx => new ErrorResponse(StatusCodes.Status400BadRequest, description: argEx.Message),
                _ => new ErrorResponse(StatusCodes.Status500InternalServerError, description: "An unexpected error occurred")
            };

            response.StatusCode = errorResponse.StatusCode;
            
            var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await response.WriteAsync(jsonResponse);
        }
    }
}
