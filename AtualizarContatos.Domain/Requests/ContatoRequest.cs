using System.ComponentModel.DataAnnotations;

namespace AtualizarContatos.Domain.Requests
{
    public class ContatoRequest
    {
        [Required(ErrorMessage = "O nome do contato é obrigatório.")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ]+(\s[a-zA-ZÀ-ÿ]+)+$", ErrorMessage = "O nome deve conter nome e sobrenome, separados por um espaço.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "O telefone do contato é obrigatório.")]
        [RegularExpression(@"^\d{8,15}$", ErrorMessage = "O telefone deve conter entre 8 e 15 dígitos.")]
        public required string Telefone { get; set; }

        [Required(ErrorMessage = "O email do contato é obrigatório.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "O e-mail fornecido é inválido.")]
        public required string Email { get; set; }

        [Range(1, 99, ErrorMessage = "O DDD fornecido é inválido.")]
        public int DDD { get; set; }
    }
}
