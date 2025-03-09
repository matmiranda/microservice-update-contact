using AtualizarContatos.Domain.Requests;

namespace AtualizarContatos.Service.Contato
{
    public interface IContatoService
    {
        Task AdicionarContato(ContatoRequest contato);
    }
}
