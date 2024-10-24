using protocol.rabbitqm.shared.Models.Request;

namespace protocol.rabbitqm.shared.Interfaces
{
    public interface IServiceAuth
    {
        string Login(UserRequest sender);

    }
}
