using protocol.rabbitqm.shared.Interfaces;
using protocol.rabbitqm.shared.Models.Request;
using protocol.rabbitqm.test.Base;
using Xunit.Abstractions;

namespace protocol.rabbitqm.test
{
    public class ServiceAuth_Test : Base_Service_Test<Program>
    {
        private readonly IServiceAuth service;

        public ServiceAuth_Test(ITestOutputHelper outputWriter) : base(outputWriter)
        {
            service = GetService<IServiceAuth>();
        }

        [Theory]
        [InlineData("user", "123456", true)]
        [InlineData("user", "passfake", false)]
        [InlineData("userfake", "123456", false)]
        [InlineData("userfake", "passfake", false)]
        private void CalcOrderBoxAsync(string user, string pass, bool expected)
        {
            var response = service.Login(new UserRequest() { User = user, Pass = pass });
            Assert.True((response != null) == expected);
        }
    }
}
