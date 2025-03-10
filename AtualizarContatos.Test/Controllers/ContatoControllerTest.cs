using AtualizarContatos.Api.Controllers;
using AtualizarContatos.Domain.Requests;
using AtualizarContatos.Infrastructure.Exceptions;
using AtualizarContatos.Service.Contato;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace AtualizarContatos.Test.Controllers
{
    [TestFixture]
    public class ContatoControllerTest : IDisposable
    {
        private Mock<IContatoService> mockContatoService;
        private ContatoController contatoController;

        [SetUp]
        public void SetUp()
        {
            // Arrange: Inicializa o mock do IContatoService
            mockContatoService = new Mock<IContatoService>();
            contatoController = new ContatoController(mockContatoService.Object);
        }

        [TearDown]
        public void TearDown()
        {
            // Dispose the contatoController
            contatoController.Dispose();
        }

        //[Test]
        //public async Task PostContato_ShouldReturnAccepted_WhenValidRequest()
        //{
        //    // Arrange: Cria um objeto de request válido
        //    var contatoRequest = new ContatoRequest
        //    {
        //        Nome = "Nome Teste",
        //        Telefone = "123456789",
        //        Email = "teste@teste.com",
        //        DDD = 11
        //    };

        //    // Ação: Simula o comportamento do método AdicionarContato
        //    mockContatoService.Setup(service => service.AdicionarContato(It.IsAny<ContatoRequest>()))
        //                       .Returns(Task.CompletedTask);

        //    // Ação: Chama o método PostContato
        //    var result = await contatoController.PostContato(contatoRequest);

        //    // Assert: Verifica se o resultado é do tipo ActionResult e contém um AcceptedResult
        //    Assert.That(result.Result, Is.InstanceOf<AcceptedResult>());
        //}

        //[Test]
        //public void PostContato_ShouldReturnBadRequest_WhenExceptionIsThrown()
        //{
        //    // Arrange: Cria um objeto de request válido
        //    var contatoRequest = new ContatoRequest
        //    {
        //        Nome = "Nome Teste",
        //        Telefone = "123456789",
        //        Email = "teste@teste.com",
        //        DDD = 11
        //    };

        //    // Ação: Simula uma exceção no serviço
        //    mockContatoService.Setup(service => service.AdicionarContato(It.IsAny<ContatoRequest>()))
        //                       .ThrowsAsync(new CustomException(HttpStatusCode.Conflict, "Contato com este email já existe."));

        //    // Ação: Chama o método PostContato
        //    var ex = Assert.ThrowsAsync<CustomException>(async () => await contatoController.PostContato(contatoRequest));

        //    // Assert: Verifica a mensagem da exceção
        //    Assert.That(ex.Message, Is.EqualTo("Contato com este email já existe."));
        //}

        public void Dispose()
        {
            // Dispose the contatoController
            contatoController?.Dispose();
        }
    }
}
