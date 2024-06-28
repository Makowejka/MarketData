using MarketData.Api.Validators;
using FluentAssertions;
using MarketData.Domain.Dto;
using FluentValidation.TestHelper;
using MarketData.Domain.Options;
using Microsoft.Extensions.Options;
using Moq;

namespace MarketData.Api.Tests;

public class GetMarketDataDtoValidatorTest
{
    private readonly GetMarketDataDtoValidator _validator;

    public GetMarketDataDtoValidatorTest()
    {
        var assetsOptionsMock = new Mock<IOptions<AssetsOptions>>();

        assetsOptionsMock
            .SetupGet(x => x.Value)
            .Returns(new AssetsOptions
            {
                AvailableAssets = ["USD"]
            });

        _validator = new GetMarketDataDtoValidator(assetsOptionsMock.Object);
    }

    public static IEnumerable<object[]> TestData()
    {
        yield return
        [
            new GetMarketDataDto(null!, new DateTime(2024, 6, 19), new DateTime(2024, 6, 19, 1, 1, 1)),
            new List<string> { "'Asset' must not be empty." }
        ];
        yield return
        [
            new GetMarketDataDto("", new DateTime(2024, 6, 19), new DateTime(2024, 6, 20)),
            new List<string> { "'Asset' must not be empty." }
        ];
        yield return
        [
            new GetMarketDataDto("blablabla", new DateTime(2024, 6, 19), new DateTime(2024, 6, 20)),
            new List<string> { "Asset blablabla is not supported" }
        ];
        yield return
        [
            new GetMarketDataDto("USD", new DateTime(2024, 6, 23), new DateTime(2024, 5, 24)),
            new List<string>
            {
                "'Time From Utc' must be less than '24/05/2024 00:00:00'.",
                "'Time To Utc' must be greater than '23/06/2024 00:00:00'."
            }
        ];
        yield return
        [
            new GetMarketDataDto("USD", default, new DateTime(2024, 6, 20)),
            new List<string> { "'Time From Utc' must not be empty." }
        ];
        yield return
        [
            new GetMarketDataDto("USD", new DateTime(2024, 6, 22), new DateTime(2022, 6, 21)),
            new List<string>
            {
                "'Time From Utc' must be less than '21/06/2022 00:00:00'.",
                "'Time To Utc' must be greater than '22/06/2024 00:00:00'."
            }
        ];
        yield return
        [
            new GetMarketDataDto("USD", new DateTime(2022, 6, 21), default),
            new List<string>
            {
                "'Time From Utc' must be less than '01/01/0001 00:00:00'.",
                "'Time To Utc' must not be empty.",
                "'Time To Utc' must be greater than '21/06/2022 00:00:00'."
            }
        ];
        yield return
        [
            new GetMarketDataDto("", new DateTime(2021, 1, 22), default),
            new List<string>
            {
                "'Asset' must not be empty.",
                "'Time From Utc' must be less than '01/01/0001 00:00:00'.",
                "'Time To Utc' must not be empty.",
                "'Time To Utc' must be greater than '22/01/2021 00:00:00'."
            }
        ];
    }

    [Theory]
    [MemberData(nameof(TestData))]
    public async Task GetMarketDataDtoValidator(GetMarketDataDto dto, List<string> expectedErrors)
    {
        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.Errors.Select(x => x.ErrorMessage).Should().BeEquivalentTo(expectedErrors);
    }
}
