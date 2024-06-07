namespace MarketData.Domain.Dto;

public record GetMarketDataDto(string Asset, DateTime TimeUtc, decimal Price);
