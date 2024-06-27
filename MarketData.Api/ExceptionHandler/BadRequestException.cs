using FluentValidation.Results;

namespace MarketData.Api.ExceptionHandler;

public class BadRequestException : Exception
{
    public string ValidationMessage { get; private init; }

    public BadRequestException(List<ValidationFailure> errors)
    {
        ValidationMessage = string.Join(',', errors.Select(x => x.ErrorMessage));
    }
}
