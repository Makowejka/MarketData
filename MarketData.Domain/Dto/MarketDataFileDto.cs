using MarketData.Domain.Model;

namespace MarketData.Domain.Dto;

public record MarketDataFileDto(
    DateTime StartedAtUtc,
    DateTime? CompletedAtUtc,
    string FilePath,
    MarketDataFileUploadStatus Status,
    string? ErrorMessage);
