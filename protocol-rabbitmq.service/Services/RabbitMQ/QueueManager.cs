using protocol.rabbitmq.shared.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace protocol.rabbitmq.service.Services.RabbitMQ
{
    public class QueueManager
    {
        private readonly IModel _channel;
        private readonly IServiceLog _log;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public QueueManager(IModel channel, IServiceLog log)
        {
            _channel = channel;
            _log = log;
        }

        public async Task HandleMessageAsync(BasicDeliverEventArgs e, MessageProcessor messageProcessor)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (e.Body.Length == 0) { return; }

                await messageProcessor.ProcessMessageAsync(e.Body.ToArray());
                _channel.BasicAck(e.DeliveryTag, false);
                _log.Info("Fila de Protocolos Processada.");
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
