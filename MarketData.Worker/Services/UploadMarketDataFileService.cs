using FluentValidation;
using MarketData.Domain.Messages;
using MarketData.Worker.Contract;
using Npgsql;
using Sylvan.Data.Csv;
using CsvDataReader = Sylvan.Data.Csv.CsvDataReader;

namespace MarketData.Worker.Services;

public class UploadMarketDataFileService : IUploadMarketDataFileService
{
    private readonly ILogger<UploadMarketDataFileService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IValidator<UploadMarketDataFileMessage> _validator;

    public UploadMarketDataFileService(
        ILogger<UploadMarketDataFileService> logger,
        IConfiguration configuration,
        IValidator<UploadMarketDataFileMessage> validator)
    {
        _logger = logger;
        _configuration = configuration;
        _validator = validator;
    }

    public async Task UploadAsync(UploadMarketDataFileMessage message, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(message, ct);

        if (!validationResult.IsValid)
        {
            var error = string.Join(',', validationResult.Errors.Select(x => x.ErrorMessage));
            _logger.LogError("Upload validation failed! Errors:{error}", error);

            return;
        }

        _logger.LogInformation("Uploading file path: {0}", message.FilePath);

        var watch = System.Diagnostics.Stopwatch.StartNew();

        await ImportFileAsync(message.FilePath!, ct);

        watch.Stop();

        _logger.LogInformation("Uploaded file path: {0} in {1:g}", message.FilePath, watch.Elapsed);
    }

    private async Task ImportFileAsync(string filePath, CancellationToken ct)
    {
        await using var connection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        try
        {
            const string command = $"copy \"{nameof(Domain.Entities.MarketData)}\" from STDIN (FORMAT BINARY)";

            await using var csvReader =
                await CsvDataReader.CreateAsync(filePath, new CsvDataReaderOptions(), ct);

            await connection.OpenAsync(ct);
            await using var writer = await connection.BeginBinaryImportAsync(command, ct);

            var asset = GetAssetFromFileName(filePath);

            while (await csvReader.ReadAsync(ct))
            {
                var timeUtc = csvReader.GetDateTime(0);
                DateTime.SpecifyKind(timeUtc, DateTimeKind.Utc);

                var price = csvReader.GetDecimal(1);

                await writer.WriteRowAsync(ct, [asset, timeUtc, price]);
            }

            await writer.CompleteAsync(ct);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Uploading file: {FilePath} failed!", filePath);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    private string GetAssetFromFileName(string filePath)
    {
        const int symbolCodeLength = 6;
        var fileName = Path.GetFileNameWithoutExtension(filePath);

        return fileName[..symbolCodeLength];
    }
}
