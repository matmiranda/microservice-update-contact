using AtualizarContatos.Domain.Models.RabbitMq;
using AtualizarContatos.Domain.Requests;
using AtualizarContatos.Domain.Responses;
using AtualizarContatos.Infrastructure.Exceptions;
using AtualizarContatos.Service.Contato;
using AtualizarContatos.Service.RabbitMq;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;

namespace AtualizarContatos.Test.Services
{
    [TestFixture]
    public class ContatoServiceTest
    {
        private Mock<IRabbitMqPublisherService> mockRabbitMqPublisherService;
        private Mock<IConfiguration> mockConfiguration;
        private Mock<HttpMessageHandler> mockHttpMessageHandler;
        private HttpClient httpClient;
        private ContatoService contatoService;

        [SetUp]
        public void SetUp()
        {
            mockRabbitMqPublisherService = new Mock<IRabbitMqPublisherService>();
            mockConfiguration = new Mock<IConfiguration>();

            // Simula a chave da API
            mockConfiguration.Setup(config => config["ApiAzure:Key"]).Returns("fake-key");

            // Mocka o HttpMessageHandler para simular chamadas HTTP
            mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://fiap-api-gateway.azure-api.net/")
            };

            contatoService = new ContatoService(mockRabbitMqPublisherService.Object, httpClient, mockConfiguration.Object);
        }

        [TearDown]
        public void TearDown()
        {
            httpClient.Dispose();
        }

        [Test]
        public void AtualizarContato_ShouldThrowException_WhenContatoDoesNotExist()
        {
            // Arrange
            var contatoRequest = new ContatoRequest
            {
                Id = 999,
                DDD = 11,
                Nome = "Nome Teste",
                Telefone = "123456789",
                Email = "teste@teste.com"
            };

            // Simula uma resposta 404 (contato não encontrado)
            var mockResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(mockResponse);

            // Act & Assert
            var exception = Assert.ThrowsAsync<CustomException>(() => contatoService.AtualizarContato(contatoRequest));
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.Message, Is.EqualTo("O id do contato não existe."));
        }

        [Test]
        public async Task AtualizarContato_ShouldPublishMessage_WhenContatoExistsAndDDDIsSame()
        {
            // Arrange
            var contatoRequest = new ContatoRequest
            {
                Id = 1,
                DDD = 11,
                Nome = "Nome Teste",
                Telefone = "123456789",
                Email = "teste@teste.com"
            };
            var contatoIdResponse = new ContatoIdResponse { Id = 1, DDD = 11, Regiao = "4" };

            // Simula a API retornando um contato válido
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(contatoIdResponse), Encoding.UTF8, "application/json")
            };
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(mockResponse);

            // Act
            await contatoService.AtualizarContato(contatoRequest);

            // Assert
            mockRabbitMqPublisherService.Verify(service =>
                service.PublicarContatoAsync(It.IsAny<ContactMessage>()), Times.Once);
        }

        [Test]
        public async Task AtualizarContato_ShouldPublishMessage_WhenContatoExistsAndDDDChanged()
        {
            // Arrange
            var contatoRequest = new ContatoRequest
            {
                Id = 1,
                DDD = 41,
                Nome = "Nome Teste",
                Telefone = "123456789",
                Email = "teste@teste.com"
            };
            var contatoIdResponse = new ContatoIdResponse { Id = 1, DDD = 11, Regiao = "4" };

            // Simula a API retornando um contato válido
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(contatoIdResponse), Encoding.UTF8, "application/json")
            };
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(mockResponse);

            // Act
            await contatoService.AtualizarContato(contatoRequest);

            // Assert
            mockRabbitMqPublisherService.Verify(service =>
                service.PublicarContatoAsync(It.IsAny<ContactMessage>()), Times.Once);
        }

        [Test]
        public void ObtemRegiaoPorDDD_ShouldThrowException_WhenDDDIsInvalid()
        {
            int dddInvalido = 999;

            var exception = Assert.Throws<CustomException>(() => ContatoService.ObtemRegiaoPorDDD(dddInvalido));
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(exception.Message, Is.EqualTo("Região NÃO ENCONTRADA para o DDD: 999"));
        }

        [Test]
        public void ObtemRegiaoPorDDD_ShouldReturnRegion_WhenDDDIsValid()
        {
            int dddValido = 11;

            var regiao = ContatoService.ObtemRegiaoPorDDD(dddValido);

            Assert.That(regiao, Is.EqualTo("4"));
        }
    }
}
