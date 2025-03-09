using AtualizarContatos.Domain.Enum;
using AtualizarContatos.Domain.Models.RabbitMq;
using AtualizarContatos.Domain.Requests;

namespace AtualizarContatos.Service.Mapper
{
    public static class ContatoMapper
    {
        public static ContactMessage ToContactMessage(ContatoRequest request, string regiao)
        {
            return new ContactMessage
            {
                Nome = request.Nome,
                Telefone = request.Telefone,
                Email = request.Email,
                DDD = request.DDD,
                Regiao = (RegiaoEnum)Enum.Parse(typeof(RegiaoEnum), regiao),
                CreatedAt = DateTime.UtcNow.AddHours(-3)
            };
        }
    }
}
