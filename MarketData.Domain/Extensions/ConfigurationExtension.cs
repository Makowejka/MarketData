using Microsoft.Extensions.Configuration;

namespace MarketData.Domain.Extensions;

public static class ConfigurationExtension
{
    public static T BindOptions<T>(this IConfiguration configuration, string? section = null) where T : new()
    {
        var instance = new T();
        configuration.GetSection(section ?? typeof(T).Name).Bind(instance);
        return instance;
    }
}
