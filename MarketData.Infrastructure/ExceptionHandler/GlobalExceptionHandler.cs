using System.Text.Json;
using MarketData.Domain.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace MarketData.Infrastructure.ExceptionHandler;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        ProblemDetails problemDetails;

        switch (exception)
        {
            case BadRequestException badRequestException:
                _logger.LogError(
                    badRequestException,
                    "Exception occurred: {Message}",
                    badRequestException.Message);

                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Bad Request",
                    Detail = badRequestException.Message
                };
                break;

            case NotFoundException notFoundException:
                _logger.LogError(
                    notFoundException,
                    "Exception occured: {Message}",
                    notFoundException.Message);

                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Not Found",
                    Detail = notFoundException.Message
                };
                break;

            default:
                _logger.LogError(exception,
                    "Exception occured: {Message}",
                    exception.Message);

                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Server Error"
                };
                break;
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await WriteProblemDetailsAsync(httpContext, problemDetails, cancellationToken);
        return true;
    }

    private async Task WriteProblemDetailsAsync(HttpContext httpContext, ProblemDetails problemDetails, CancellationToken cancellationToken)
    {
        try
        {
            var problemDetailsJson = JsonSerializer.Serialize(problemDetails);
            await httpContext.Response.WriteAsync(problemDetailsJson, cancellationToken);
        }
        catch (JsonException e)
        {
            _logger.LogError(e, "Error occurred during JSON serialization: {Message}", e.Message);
            await httpContext.Response.WriteAsync("An error occurred", cancellationToken);
        }
    }
}
