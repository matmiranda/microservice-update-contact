using AtualizarContatos.Domain.Enum;

namespace AtualizarContatos.Domain.Models.RabbitMq
{
    public class ContactMessage
    {
        public string? Nome { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public int DDD { get; set; }
        public RegiaoEnum Regiao { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
