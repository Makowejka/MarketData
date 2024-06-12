using MarketData.Domain.Contract;
using Microsoft.AspNetCore.Http;

namespace MarketData.Infrastructure.ExceptionHandler;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IExceptionHandler _exceptionHandler;

    public ExceptionHandlingMiddleware(RequestDelegate next, IExceptionHandler exceptionHandler)
    {
        _next = next;
        _exceptionHandler = exceptionHandler;
    }

    public async Task InvokeAsync(HttpContext context, IExceptionHandler exceptionHandler)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await _exceptionHandler.TryHandleAsync(context, ex, default);
        }
    }

}
