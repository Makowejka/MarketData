namespace MarketData.Domain.Dto;

public record MarketDataDto(string Asset, DateTime TimeUtc, decimal Price);
