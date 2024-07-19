using Dapper;
using MarketData.Domain.Contract;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace MarketData.Infrastructure.Repositories;

public class MarketDataFilesRepository : IMarketDataFilesRepository
{
    private readonly IConfiguration _configuration;

    public MarketDataFilesRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool Exists(string filePath) => File.Exists(filePath);

    public async Task<bool> IsUploadedAsync(string filePath, CancellationToken ct)
    {
        const string sql =
            """
            
                SELECT CASE WHEN EXISTS (
                    SELECT *
                    FROM "MarketDataFile"
                    WHERE "FilePath" = @filePath
                )
                THEN CAST(1 AS BIT)
                ELSE CAST(0 AS BIT) END
                        
            """;

        await using var connection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        var result = await connection.ExecuteScalarAsync<bool>(sql, new { filePath });

        return result;
    }
}
