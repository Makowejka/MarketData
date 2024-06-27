namespace MarketData.Domain.Dto;

public record GetMarketDataDto(string Asset, DateTime TimeFromUtc, DateTime TimeToUtc);
