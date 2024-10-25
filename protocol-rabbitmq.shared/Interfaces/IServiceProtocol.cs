using protocol.rabbitmq.shared.Models.DTOs;

namespace protocol.rabbitmq.shared.Interfaces
{
    public interface IServiceProtocol
    {
        Task<Result<bool>> AddProtocolAsync(ProtocolDTO? protocols);
        Task<Result<List<bool>>> AddProtocolAsync(List<ProtocolDTO>? protocol);
        Task<int> GetNextProtocolSequenceNumberAsync(string cpf);
        Task<bool> IsProtocolRegisteredAsync(string cpf, string protocol);
        Task<Result<List<ProtocolDTO>>> SearchProtocolsAsync(string doc);
    }
}
