namespace AtualizarContatos.Domain.Responses
{
    public class ContatoIdResponse
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public int DDD { get; set; }
        public string? Regiao { get; set; }
    }
}
