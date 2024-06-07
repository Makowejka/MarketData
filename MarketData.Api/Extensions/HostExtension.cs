using MarketData.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MarketData.Api.Extensions;

public static class HostExtension
{
    public static async Task MigrateDataBaseAsync(this IHost host)
    {
        await using var scope = host.Services.CreateAsyncScope();

        await using var context = scope.ServiceProvider.GetRequiredService<DataContext>();

        await context.Database.MigrateAsync();
    }
}
