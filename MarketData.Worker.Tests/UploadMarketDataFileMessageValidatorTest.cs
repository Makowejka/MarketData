using FluentAssertions;
using FluentValidation.TestHelper;
using MarketData.Domain.Contract;
using MarketData.Domain.Messages;
using MarketData.Worker.Validators;
using Moq;

namespace MarketData.Worker.Tests;

public class UploadMarketDataFileValidatorTest
{
    private readonly Mock<IMarketDataFilesRepository> _repositoryMock;

    private readonly UploadMarketDataFileMessageValidator _validator;

    public UploadMarketDataFileValidatorTest()
    {
        _repositoryMock = new Mock<IMarketDataFilesRepository>();
        _validator = new UploadMarketDataFileMessageValidator(_repositoryMock.Object);
    }

    [Theory]
    [MemberData(nameof(TestData))]
    public async Task UploadMarketDataFileMessageValidator(UploadMarketDataFileMessage message, bool exists,
        bool isUploaded, List<string> expectedErrors)
    {
        // Arrange
        _repositoryMock
            .Setup(x => x.Exists(message.FilePath!))
            .Returns(exists);

        _repositoryMock
            .Setup(x => x.IsUploadedAsync(message.FilePath!, It.IsAny<CancellationToken>()))
            .ReturnsAsync(isUploaded);

        // Act
        var result = await _validator.TestValidateAsync(message);

        // Assert
        result.Errors.Select(x => x.ErrorMessage).Should().BeEquivalentTo(expectedErrors);
    }

    public static IEnumerable<object[]> TestData()
    {
        yield return
        [
            new UploadMarketDataFileMessage(""),
            true,
            true,
            new List<string> { "'File Path' must not be empty." }
        ];
        yield return
        [
            new UploadMarketDataFileMessage("/Users/goskaem/Desktop/MarketDataFiles"),
            false,
            true,
            new List<string> { "File '/Users/goskaem/Desktop/MarketDataFiles' not exists" }
        ];
        yield return
        [
            new UploadMarketDataFileMessage("/Users/goskaem/Desktop/MarketDataFiles"),
            true,
            true,
            new List<string> { "File: '/Users/goskaem/Desktop/MarketDataFiles' is already uploaded" }
        ];
        yield return
        [
            new UploadMarketDataFileMessage("/Users/goskaem/Desktop/MarketDataFiles"),
            true,
            false,
            new List<string>()
        ];
    }
}
