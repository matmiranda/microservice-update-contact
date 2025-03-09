using AtualizarContatos.Domain.Requests;
using AtualizarContatos.Domain.Responses;
using AtualizarContatos.Service.Contato;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AtualizarContatos.Api.Controllers
{
    /// <summary>
    /// Controller responsável pelo cadastro de contatos.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ContatoController : Controller
    {
        private readonly IContatoService contatoService;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="ContatoController"/>.
        /// </summary>
        /// <param name="contatoService">O serviço de cadastro de contatos.</param>
        public ContatoController(IContatoService contatoService)
        {
            this.contatoService = contatoService;
        }

        /// <summary>
        /// Adiciona um novo contato.
        /// </summary>
        /// <param name="contato">Os dados do contato a ser adicionado.</param>
        /// <returns>O contato adicionado.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [SwaggerResponse(400, "Erro de validação", typeof(ExceptionResponse), "application/json")]
        [SwaggerResponse(409, "Erro de validação", typeof(ExceptionResponse), "application/json")]
        public async Task<ActionResult<ContatoResponse>> PostContato(ContatoRequest contato)
        {
            await contatoService.AdicionarContato(contato);
            return Accepted();
        }
    }
}
