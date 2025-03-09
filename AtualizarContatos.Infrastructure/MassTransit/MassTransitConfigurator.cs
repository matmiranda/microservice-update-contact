using AtualizarContatos.Domain.Models.RabbitMq;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AtualizarContatos.Infrastructure.MassTransit
{
    public static class MassTransitConfigurator
    {
        public static void ConfigureMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            // Registra as configurações do RabbitMQ corretamente
            services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMQSettings").Bind);

            // Adiciona MassTransit
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((context, cfg) =>
                {
                    var rabbitMqSettings = context.GetService<IOptions<RabbitMqSettings>>()?.Value
                        ?? throw new InvalidOperationException("RabbitMQ settings not configured corretamente.");

                    cfg.Host(rabbitMqSettings.Host, h =>
                    {
                        h.Username(rabbitMqSettings.Username);
                        h.Password(rabbitMqSettings.Password);
                    });
                });
            });
        }
    }
}
