using protocol.rabbitmq.shared.Interfaces;
using protocol.rabbitmq.shared.Models.Request;
using protocol.rabbitmq.test.Base;
using Xunit.Abstractions;

namespace protocol.rabbitmq.test
{
    public class AuthenticationServiceTests : Base_Service_Test<Program>
    {
        private readonly IServiceAuth service;

        public AuthenticationServiceTests(ITestOutputHelper outputWriter) : base(outputWriter)
        {
            service = GetService<IServiceAuth>();
        }

        [Theory]
        [InlineData("user", "123456", true)]
        [InlineData("user", "passfake", false)]
        [InlineData("userfake", "123456", false)]
        [InlineData("userfake", "passfake", false)]
        private void Login_ShouldReturnValidResponse_WhenCredentialsAreValid(string user, string pass, bool expected)
        {
            // Arrange
            var request = new UserRequest { User = user, Pass = pass };
            // Act
            var response = service.Login(request);
            // Assert
            Assert.NotNull(response);
            // expected é o valor esperado para o teste
            Assert.True(response.success == expected); // Exemplo: Verificando se o login foi bem-sucedido ou não
        }
    }
}
