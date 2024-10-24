using protocol.rabbitqm.shared.Models.DTOs;

namespace protocol.rabbitqm.shared.Interfaces
{
    public interface IServiceProtocol
    {
        Task<Result<bool>> AddProtocolAsync(ProtocolDTO protocol);
    }
}
