using AtualizarContatos.Domain.Models.RabbitMq;
using AtualizarContatos.Service.RabbitMq;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace AtualizarContatos.Test.Service.RabbitMq
{
    [TestFixture]
    public class RabbitMqPublisherServiceTest
    {
        private Mock<ILogger<RabbitMqPublisherService>> mockLogger;
        private Mock<IBus> mockBus;
        private Mock<ISendEndpoint> mockSendEndpoint;
        private Mock<IConfiguration> mockConfiguration;
        private RabbitMqPublisherService rabbitMqPublisherService;

        [SetUp]
        public void SetUp()
        {
            // Mock do logger
            mockLogger = new Mock<ILogger<RabbitMqPublisherService>>();

            // Mock do IBus
            mockBus = new Mock<IBus>();

            // Mock do ISendEndpoint
            mockSendEndpoint = new Mock<ISendEndpoint>();

            // Mock da IConfiguration
            mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(config => config["RabbitMQSettings:QueueName"]).Returns("cadastro_contatos");

            // Instancia o serviço com mocks
            rabbitMqPublisherService = new RabbitMqPublisherService(mockLogger.Object, mockBus.Object, mockConfiguration.Object);
        }

        [Test]
        public async Task PublicarContatoAsync_ShouldSendMessage_WhenValidRequest()
        {
            // Arrange: Cria um objeto de mensagem válido
            var contactMessage = new ContactMessage
            {
                Nome = "Nome Teste",
                Telefone = "123456789",
                Email = "teste@teste.com",
                DDD = 11
            };

            // Mock de GetSendEndpoint para retornar o mock do ISendEndpoint
            mockBus.Setup(bus => bus.GetSendEndpoint(It.IsAny<Uri>())).ReturnsAsync(mockSendEndpoint.Object);

            // Ação: Chama o método PublicarContatoAsync
            await rabbitMqPublisherService.PublicarContatoAsync(contactMessage);

            // Assert: Verifica se o método Send foi chamado no endpoint
            mockSendEndpoint.Verify(endpoint => endpoint.Send(It.Is<ContactMessage>(msg => msg.Email == "teste@teste.com" && msg.Nome == "Nome Teste"), It.IsAny<CancellationToken>()), Times.Once);

            // Verifica se o Log foi chamado com a mensagem correta
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v != null && v.ToString()!.Contains("Mensagem publicada com sucesso.")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }


        [Test]
        public void PublicarContatoAsync_ShouldThrowException_WhenErrorOccurs()
        {
            // Arrange: Cria um objeto de mensagem válido
            var contactMessage = new ContactMessage
            {
                Nome = "Nome Teste",
                Telefone = "123456789",
                Email = "teste@teste.com",
                DDD = 11
            };

            // Simula erro no GetSendEndpoint
            mockBus.Setup(bus => bus.GetSendEndpoint(It.IsAny<Uri>())).ThrowsAsync(new Exception("Erro ao obter endpoint"));

            // Ação & Assert: Verifica se uma exceção é lançada
            var ex = Assert.ThrowsAsync<Exception>(async () => await rabbitMqPublisherService.PublicarContatoAsync(contactMessage));
            Assert.That(ex!.Message, Is.EqualTo("Erro ao obter endpoint"));

            // Verifica se o log de erro foi chamado
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v != null && v.ToString()!.Contains("Erro ao publicar mensagem no RabbitMQ.")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((state, ex) => true)),
                Times.Once);
        }

    }
}
