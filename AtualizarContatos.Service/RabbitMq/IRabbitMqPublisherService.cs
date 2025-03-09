using AtualizarContatos.Domain.Models.RabbitMq;

namespace AtualizarContatos.Service.RabbitMq
{
    public interface IRabbitMqPublisherService
    {
        Task PublicarContatoAsync(ContactMessage contactMessage);
    }
}
