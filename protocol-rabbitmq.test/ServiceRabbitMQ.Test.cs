using Moq;
using protocol.rabbitmq.service.Services;
using protocol.rabbitmq.service.Services.RabbitMQ;
using protocol.rabbitmq.shared.Helpers;
using protocol.rabbitmq.shared.Interfaces;
using protocol.rabbitmq.shared.Models.DTOs;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace protocol.rabbitmq.test.Services.RabbitMQ
{
    public class ServiceRabbitMQTests
    {
        private readonly Mock<QueueManager> _mockQueueManager;
        private readonly Mock<IModel> _mockChannel;
        private readonly Mock<MessageProcessor> _mockMessageProcessor;
        private readonly Mock<IServiceLog> _mockLogger;
        private readonly ServiceRabbitMQ _serviceRabbitMQ;

        public ServiceRabbitMQTests()
        {
            _mockQueueManager = new Mock<QueueManager>();
            _mockChannel = new Mock<IModel>();
            _mockMessageProcessor = new Mock<MessageProcessor>();
            _mockLogger = new Mock<IServiceLog>();
            _serviceRabbitMQ = new ServiceRabbitMQ(_mockQueueManager.Object, _mockChannel.Object, _mockMessageProcessor.Object, _mockLogger.Object);
        }

        [Fact]
        public void IsServerAccessible_ShouldReturnSuccess_WhenPortIsOpen()
        {
            // Arrange (Não é necessário, pois o método usa Helper.NetWork.PortIsOpenByUrl diretamente)

            // Act
            var result = _serviceRabbitMQ.IsServerAccessible();

            // Assert
            Assert.True(result.success);
            Assert.Equal("Servidor do RabbitMQ está disponível", result.xmessage);
        }

        [Fact]
        public void IsServerAccessible_ShouldReturnFail_WhenPortIsClosed()
        {
            // Act
            var result = _serviceRabbitMQ.IsServerAccessible();

            // Assert
            Assert.False(result.success);
            Assert.Equal("Servidor do RabbitMQ não está disponível", result.xmessage);
        }

        [Fact]
        public async Task PublishAsync_ShouldPublishMessagesAndReturnSuccess_WhenServerIsAccessible()
        {
            // Arrange
            var protocolCollection = new List<ProtocolDTO>
            {
                new ProtocolDTO { CPF = "123", NumProtocol = "P001" },
                new ProtocolDTO { CPF = "456", NumProtocol = "P002" }
            };

            // Act
            var result = await _serviceRabbitMQ.PublishAsync(protocolCollection);

            // Assert
            Assert.True(result.success);
            Assert.Equal($"Enviado {protocolCollection.Count} protocolos para a fila de processamento", result.xmessage);
            _mockChannel.Verify(channel => channel.BasicPublish(
                Helper.Constants.RABBITMQ_EXCHANGENAME,
                Helper.Constants.RABBITMQ_ROUTINGKEY,
                null,
                It.IsAny<byte[]>()
            ), Times.Exactly(protocolCollection.Count));
        }

        [Fact]
        public void TotalMessagesPending_ShouldGetMessageCountFromQueue()
        {
            // Arrange
            var queueName = Helper.Constants.RABBITMQ_QUEUENAME;
            _mockChannel.Setup(channel => channel.QueueDeclarePassive(queueName))
                .Returns(It.IsAny<QueueDeclareOk>());

            // Act
            _serviceRabbitMQ.TotalMessagesPending(queueName);

            // Assert
            _mockChannel.Verify(channel => channel.QueueDeclarePassive(queueName), Times.Once);
            // Não podemos verificar o valor de TotalMessages diretamente, pois depende da implementação de QueueDeclareOk
        }

        [Fact]
        public void StartConsumer_ShouldStartConsumerAndWaitForMessages_WhenServerIsAccessible()
        {
            // Arrange
            _mockChannel.Setup(channel => channel.QueueDeclarePassive(Helper.Constants.RABBITMQ_QUEUENAME))
                .Returns(new QueueDeclareOk(Helper.Constants.RABBITMQ_QUEUENAME, 0, 0)); // Simulando fila vazia

            // Act
            var result = _serviceRabbitMQ.StartConsumer();

            // Assert
            Assert.True(result.success);
            Assert.Equal("Mensagens processadas.", result.xmessage);
            _mockChannel.Verify(channel => channel.BasicConsume(
                Helper.Constants.RABBITMQ_QUEUENAME,
                false,
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<IDictionary<string, object>>(),
                It.IsAny<EventingBasicConsumer>()
            ), Times.Once);
        }
    }
}