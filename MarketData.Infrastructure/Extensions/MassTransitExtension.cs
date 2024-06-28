using System.Reflection;
using MarketData.Domain.Extensions;
using MarketData.Infrastructure.Options;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MarketData.Infrastructure.Extensions;

public static class MassTransitExtension
{
    public static void AddMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(config =>
            {
                config.AddConsumers(Assembly.GetEntryAssembly());
                config.SetKebabCaseEndpointNameFormatter();

                config.UsingRabbitMq((context, rabbitConfig) =>
                    {
                        var options = configuration.BindOptions<MassTransitOptions>();

                        rabbitConfig.Host(options.Host, options.Port, "/", h =>
                        {
                            if (string.IsNullOrWhiteSpace(options.Username)
                                || string.IsNullOrWhiteSpace(options.Password))
                            {
                                return;
                            }

                            h.Username(options.Username);
                            h.Password(options.Password);
                        });

                        rabbitConfig.ConfigureEndpoints(context);
                    }
                );
            }
        );
    }
}
