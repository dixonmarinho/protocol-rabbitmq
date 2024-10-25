using protocol.rabbitmq.shared.Models.DTOs;

namespace protocol.rabbitmq.shared.Interfaces
{
    public interface IServiceRabbitMQ
    {
        Task<Result<bool>> PublishAsync(List<ProtocolDTO> protocol);
        Result<bool> StartConsumer();
        Result<bool> IsServerAccessible();
    }
}
