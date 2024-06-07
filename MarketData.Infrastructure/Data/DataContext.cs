using MarketData.Domain.Entities;
using MarketData.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace MarketData.Infrastructure.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Domain.Entities.MarketData> MarketData { get; set; } = null!;

    public DbSet<MarketDataFile> MarketDataFile { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder
            .Entity<MarketDataFile>()
            .Property(x => x.Status)
            .HasConversion(x => x.ToString(), x => Enum.Parse<MarketDataFileUploadStatus>(x));
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        builder.Properties<string>().HaveMaxLength(1000);
        builder.Properties<decimal>().HavePrecision(18, 6);
    }
}
