using Newtonsoft.Json;
using protocol.rabbitmq.shared.Interfaces;
using protocol.rabbitmq.shared.Models.DTOs;
using System.Text;

namespace protocol.rabbitmq.service.Services.RabbitMQ
{
    // Classe responsável por processar a mensagem
    public class MessageProcessor
    {
        private readonly IServiceProtocol _service;
        private readonly IServiceLog _log;

        public MessageProcessor(IServiceProtocol service, IServiceLog log)
        {
            _service = service;
            _log = log;
        }

        public async Task ProcessMessageAsync(byte[] messageBody)
        {
            try
            {
                var message = Encoding.UTF8.GetString(messageBody);
                var protocol = JsonConvert.DeserializeObject<ProtocolDTO>(message);
                await _service.AddProtocolAsync(protocol);
                _log.Info($"Protocolo {protocol.NumProtocol} Enviado para File de Processamento.");
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }
    }
}
