using protocol.rabbitqm.shared.Interfaces;
using protocol.rabbitqm.shared.Models.DTOs;
using protocol.rabbitqm.test.Base;
using Xunit.Abstractions;

namespace protocol.rabbitqm.test
{
    public class ServiceProtocol_Test : Base_Service_Test<Program>
    {
        private readonly IServiceProtocol protocol;
        public ServiceProtocol_Test(ITestOutputHelper outputWriter) : base(outputWriter)
        {
            protocol = GetService<IServiceProtocol>();
        }

        [Fact]
        public async Task AddProtocolAsync_Test()
        {
            // Arrange
            var protocolDTO = new ProtocolDTO
            {
                NumProtocol = "123",
                NumViaDocumento = "123",
                CPF = "123",
                RG = "123",
                Nome = "123",
                NomeMae = "123",
                NomePai = "123",
                Foto = null
            };

            // Act
            var result = await protocol.AddProtocolAsync(protocolDTO);

            // Assert
            Assert.True(result.success);
        }
    }
}
