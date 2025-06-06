﻿using AtualizarContatos.Domain.Requests;
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
    [Route("atualizar/contato")]
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
        /// Atualiza um contato existente.
        /// </summary>
        /// <param name="contato">Os dados do contato a ser atualizado.</param>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(400, "Erro de validação", typeof(ExceptionResponse), "application/json")]
        [SwaggerResponse(404, "Erro de validação", typeof(ExceptionResponse), "application/json")]
        public async Task<IActionResult> PutContato(ContatoRequest contato)
        {
            await contatoService.AtualizarContato(contato);
            return Accepted();
        }
    }
}
