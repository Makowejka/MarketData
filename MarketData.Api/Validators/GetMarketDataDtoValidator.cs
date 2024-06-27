using MarketData.Domain.Dto;
using MarketData.Domain.Options;
using Microsoft.Extensions.Options;

namespace MarketData.Api.Validators;

using FluentValidation;

public class GetMarketDataDtoValidator : AbstractValidator<GetMarketDataDto>
{
    public GetMarketDataDtoValidator(IOptions<AssetsOptions> assetsOptions)
    {
        RuleFor(x => x.Asset)
            .NotEmpty();

        RuleFor(x => x.Asset)
            .Must(assetsOptions.Value.AvailableAssets.Contains)
            .When(x => string.IsNullOrWhiteSpace(x.Asset) == false)
            .WithMessage(x => $"Asset {x.Asset} is not supported");

        RuleFor(x => x.TimeFromUtc)
            .NotEmpty()
            .LessThan(x => x.TimeToUtc);

        RuleFor(x => x.TimeToUtc)
            .NotEmpty()
            .GreaterThan(x => x.TimeFromUtc);
    }
}
