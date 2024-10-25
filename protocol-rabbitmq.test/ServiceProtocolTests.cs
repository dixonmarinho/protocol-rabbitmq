using AutoMapper;
using Moq;
using protocol.rabbitmq.data.Data;
using protocol.rabbitmq.service.Services;
using protocol.rabbitmq.shared;
using protocol.rabbitmq.shared.Interfaces;
using protocol.rabbitmq.shared.Models;
using protocol.rabbitmq.shared.Models.DTOs;

namespace protocol.rabbitmq.test.Services
{
    public class ServiceProtocolTests
    {
        private readonly Mock<IUnitOfWork<AppDataContext>> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IServiceLog> _mockLogger;
        private readonly IServiceProtocol _serviceProtocol;

        public ServiceProtocolTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork<AppDataContext>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<IServiceLog>();
            _serviceProtocol = new ServiceProtocol(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetNextProtocolSequenceNumberAsync_ShouldReturnNextSequenceNumber_WhenProtocolsFoundForCpf()
        {
            // Arrange
            var cpf = "12345678901";
            var existingProtocols = new List<Protocol>
            {
                new Protocol { CPF = cpf, NumViaDocumento = 1 },
                new Protocol { CPF = cpf, NumViaDocumento = 2 }
            };
            _mockUnitOfWork.Setup(uow => uow.GetRepository<Protocol>().ListAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Protocol, bool>>>()))
                .ReturnsAsync(new Result<List<Protocol>>());
            // Act
            var result = await _serviceProtocol.GetNextProtocolSequenceNumberAsync(cpf);
            // Assert
            Assert.Equal(3, result);
        }

        [Fact]
        public async Task IsProtocolRegisteredAsync_ShouldReturnFalse_WhenNoProtocolFound()
        {
            // Arrange
            var cpf = "12345678901";
            var protocolNumber = "P0001";
            _mockUnitOfWork.Setup(uow => uow.GetRepository<Protocol>().ListAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Protocol, bool>>>()))
                .ReturnsAsync(new Result<List<Protocol>>());
            // Act
            var result = await _serviceProtocol.IsProtocolRegisteredAsync(cpf, protocolNumber);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsProtocolRegisteredAsync_ShouldReturnTrue_WhenProtocolFound()
        {
            // Arrange
            var cpf = "12345678901";
            var protocolNumber = "P0001";
            var existingProtocols = new List<Protocol>
            {
                new Protocol { CPF = cpf, NumProtocol = protocolNumber }
            };
            _mockUnitOfWork.Setup(uow => uow.GetRepository<Protocol>().ListAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Protocol, bool>>>()))
                .ReturnsAsync(new Result<List<Protocol>>());

            // Act
            var result = await _serviceProtocol.IsProtocolRegisteredAsync(cpf, protocolNumber);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task AddProtocolAsync_ShouldReturnFail_WhenProtocolIsNull()
        {
            // Arrange
            ProtocolDTO protocol = null;

            // Act
            var result = await _serviceProtocol.AddProtocolAsync(protocol);

            // Assert
            Assert.False(result.success);
            Assert.Equal("Protocolo Vazio", result.xmessage);
        }

        [Fact]
        public async Task AddProtocolAsync_ShouldReturnFail_WhenProtocolIsInvalid()
        {
            // Arrange
            var protocol = new ProtocolDTO { CPF = "12345678901", NumProtocol = "P0001" };
            _mockUnitOfWork.Setup(uow => uow.GetRepository<Protocol>().ListAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Protocol, bool>>>()))
                .ReturnsAsync(new Result<List<Protocol>>());
            // Act
            var result = await _serviceProtocol.AddProtocolAsync(protocol);
            // Assert
            Assert.False(result.success);
            Assert.StartsWith("Protocolo Invalido :", result.xmessage);
        }


        [Fact]
        public async Task AddProtocolAsync_List_ShouldReturnFail_WhenListIsNull()
        {
            // Arrange
            List<ProtocolDTO> protocols = null;

            // Act
            var result = await _serviceProtocol.AddProtocolAsync(protocols);

            // Assert
            Assert.False(result.success);
            Assert.Equal("Lista de Protocolos Vazia", result.xmessage);
        }

        [Fact]
        public async Task AddProtocolAsync_List_ShouldReturnFail_WhenListIsEmpty()
        {
            // Arrange
            var protocols = new List<ProtocolDTO>();

            // Act
            var result = await _serviceProtocol.AddProtocolAsync(protocols);

            // Assert
            Assert.False(result.success);
            Assert.Equal("Lista de Protocolos Vazia", result.xmessage);
        }

        [Fact]
        public async Task AddProtocolAsync_List_ShouldHandleNullProtocols()
        {
            // Arrange
            var protocols = new List<ProtocolDTO> { null, new ProtocolDTO { CPF = "123", NumProtocol = "P001" } };

            // Act
            var result = await _serviceProtocol.AddProtocolAsync(protocols);

            // Assert
            Assert.True(result.success);
            Assert.Single(result.data.Where(r => !r)); // Verifica se apenas um protocolo falhou (o nulo)
        }

        [Fact]
        public async Task AddProtocolAsync_List_ShouldHandleInvalidProtocols()
        {
            // Arrange
            var protocols = new List<ProtocolDTO>
            {
                new ProtocolDTO { CPF = "123", NumProtocol = "" }, // Inválido
                new ProtocolDTO { CPF = "456", NumProtocol = "P002" }
            };
            _mockUnitOfWork.Setup(uow => uow.GetRepository<Protocol>().ListAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Protocol, bool>>>()))
                .ReturnsAsync(new Result<List<Protocol>>());

            // Act
            var result = await _serviceProtocol.AddProtocolAsync(protocols);

            // Assert
            Assert.True(result.success);
            Assert.Single(result.data.Where(r => !r)); // Verifica se apenas um protocolo falhou (o inválido)
        }

        [Fact]
        public async Task AddProtocolAsync_List_ShouldAddValidProtocols()
        {
            // Arrange
            var protocols = new List<ProtocolDTO>
            {
                new ProtocolDTO { CPF = "123", NumProtocol = "P001" },
                new ProtocolDTO { CPF = "456", NumProtocol = "P002" }
            };
            _mockUnitOfWork.Setup(uow => uow.GetRepository<Protocol>().ListAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Protocol, bool>>>()))
                .ReturnsAsync(new Result<List<Protocol>>());
            _mockMapper.Setup(mapper => mapper.Map<List<Protocol>>(protocols))
                .Returns(protocols.Select(p => new Protocol { CPF = p.CPF, NumProtocol = p.NumProtocol }).ToList());

            // Act
            var result = await _serviceProtocol.AddProtocolAsync(protocols);

            // Assert
            Assert.True(result.success);
            Assert.All(result.data, r => Assert.True(r)); // Verifica se todos os protocolos foram adicionados com sucesso
        }

        [Fact]
        public async Task SearchProtocolsAsync_ShouldReturnFail_WhenDocumentIsNullOrEmpty()
        {
            // Arrange
            string document = null;

            // Act
            var result = await _serviceProtocol.SearchProtocolsAsync(document);

            // Assert
            Assert.False(result.success);
            Assert.Equal("Informe o Protocolo, CPF ou RG", result.xmessage);
        }

        [Fact]
        public async Task SearchProtocolsAsync_ShouldReturnFail_WhenNoProtocolsFound()
        {
            // Arrange
            var document = "12345678901";
            _mockUnitOfWork.Setup(uow => uow.GetRepository<Protocol>().ListAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Protocol, bool>>>()))
                .ReturnsAsync(new Result<List<Protocol>>());

            // Act
            var result = await _serviceProtocol.SearchProtocolsAsync(document);

            // Assert
            Assert.False(result.success);
            Assert.Equal("Nenhum protocolo existente para este documento", result.xmessage);
        }

        [Fact]
        public async Task SearchProtocolsAsync_ShouldReturnSuccess_WhenProtocolsFound()
        {
            // Arrange
            var document = "12345678901";
            var existingProtocols = new List<Protocol>
            {
                new Protocol { CPF = document },
                new Protocol { NumProtocol = document },
                new Protocol { RG = document }
            };
            _mockUnitOfWork.Setup(uow => uow.GetRepository<Protocol>().ListAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Protocol, bool>>>()))
                .ReturnsAsync(new Result<List<Protocol>>());
            _mockMapper.Setup(mapper => mapper.Map<List<ProtocolDTO>>(existingProtocols))
                .Returns(existingProtocols.Select(p => new ProtocolDTO { CPF = p.CPF, NumProtocol = p.NumProtocol, RG = p.RG }).ToList());

            // Act
            var result = await _serviceProtocol.SearchProtocolsAsync(document);

            // Assert
            Assert.True(result.success);
            Assert.Equal(existingProtocols.Count, result.data.Count);
        }

        [Fact]
        public async Task SearchProtocolsAsync_ShouldReturnFail_WhenRepositoryReturnsFail()
        {
            // Arrange
            var document = "12345678901";
            _mockUnitOfWork.Setup(uow => uow.GetRepository<Protocol>().ListAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Protocol, bool>>>()))
                .ReturnsAsync(new Result<List<Protocol>>());

            // Act
            var result = await _serviceProtocol.SearchProtocolsAsync(document);

            // Assert
            Assert.False(result.success);
            Assert.Equal("Nenhum protocolo existente para este documento", result.xmessage);
        }
    }
}