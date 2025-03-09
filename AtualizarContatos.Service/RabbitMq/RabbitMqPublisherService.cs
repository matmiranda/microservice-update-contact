using AtualizarContatos.Domain.Models.RabbitMq;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AtualizarContatos.Service.RabbitMq
{
    public class RabbitMqPublisherService : IRabbitMqPublisherService
    {
        private readonly ILogger<RabbitMqPublisherService> _logger;
        private readonly IBus _bus;
        private readonly string _queueName;

        public RabbitMqPublisherService(ILogger<RabbitMqPublisherService> logger, IBus bus, IConfiguration configuration)
        {
            _logger = logger;
            _bus = bus;
            _queueName = configuration["RabbitMQSettings:QueueName"] ?? throw new ArgumentNullException(nameof(configuration), "QueueName configuration is missing.");
        }

        public async Task PublicarContatoAsync(ContactMessage contactMessage)
        {
            try
            {
                _logger.LogInformation($"Publicando mensagem no RabbitMQ: {JsonSerializer.Serialize(contactMessage)}");
                var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{_queueName}"));
                await endpoint.Send(contactMessage);
                _logger.LogInformation("Mensagem publicada com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao publicar mensagem no RabbitMQ.");
                throw;
            }
        }
    }
}
