using protocol.rabbitmq.shared.Models.Request;

namespace protocol.rabbitmq.shared.Interfaces
{
    public interface IServiceAuth
    {
        Result<string> Login(UserRequest sender);

    }
}
