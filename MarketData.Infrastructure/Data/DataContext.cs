using MarketData.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketData.Infrastructure.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Domain.Entities.MarketData> MarketData { get; set; } = null!;

    public DbSet<MarketDataFile> MarketDataFile { get; set; } = null!;
}
