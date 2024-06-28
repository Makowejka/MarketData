namespace MarketData.Api.ExśceptionHandler;

public class NotFoundException : Exception
{
    public NotFoundException()
    {
    }

    public NotFoundException(string errorMessage)
        : base(errorMessage)
    {
    }

    public NotFoundException(string errorMessage, Exception inner)
        : base(errorMessage, inner)
    {
    }
}
