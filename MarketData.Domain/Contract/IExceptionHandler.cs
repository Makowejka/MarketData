using Microsoft.AspNetCore.Http;

namespace MarketData.Domain.Contract;

public interface IExceptionHandler
{
    ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken);
}
