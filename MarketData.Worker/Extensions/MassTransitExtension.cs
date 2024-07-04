using MarketData.Domain.Extensions;
using MarketData.Infrastructure.Options;
using MarketData.Worker.Consumers;
using MassTransit;

namespace MarketData.Worker.Extensions;

public static class MassTransitExtension
{
    public static void AddMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(config =>
            {
                config.AddConsumer<UploadMarketDataFileConsumer>();
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
