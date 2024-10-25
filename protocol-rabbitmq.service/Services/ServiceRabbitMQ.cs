using Newtonsoft.Json;
using protocol.rabbitmq.service.Services.RabbitMQ;
using protocol.rabbitmq.shared;
using protocol.rabbitmq.shared.Helpers;
using protocol.rabbitmq.shared.Interfaces;
using protocol.rabbitmq.shared.Models.DTOs;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace protocol.rabbitmq.service.Services
{
    public class ServiceRabbitMQ : IServiceRabbitMQ
    {
        private int TotalMessages = 0; // Total de mensagens na fila
        private bool ProcessessingData = false;
        private const int RabbitMQPort = 15672;
        private readonly ConnectionFactory _connectionFactory;
        private readonly IModel channel;
        private readonly QueueManager queue;
        private readonly MessageProcessor processor;
        private readonly IServiceLog log;

        public ServiceRabbitMQ(QueueManager queue, IModel channel, MessageProcessor processor, IServiceLog log)
        {
            this.queue = queue;
            this.channel = channel;
            this.processor = processor;
            this.log = log;
        }

        public Result<bool> IsServerAccessible()
        {
            var isAccessible = Helper.NetWork.PortIsOpenByUrl(Helper.Constants.RabbitMQHost, Helper.Constants.RabbitMQPort);
            return isAccessible
                ? Result<bool>.Success("Servidor do RabbitMQ está disponível")
                : Result<bool>.Fail("Servidor do RabbitMQ não está disponível");
        }

        public async Task<Result<bool>> PublishAsync(List<ProtocolDTO> protocolCollection)
        {
            var serverResponse = IsServerAccessible();
            if (!serverResponse.success)
                return await Result<bool>.FailAsync(serverResponse.xmessage);

            foreach (var protocol in protocolCollection)
            {
                var message = JsonConvert.SerializeObject(protocol);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: Helper.Constants.RABBITMQ_EXCHANGENAME, routingKey: Helper.Constants.RABBITMQ_ROUTINGKEY, basicProperties: null, body: body);
            }

            var logMessage = $"Enviado {protocolCollection.Count} protocolos para a fila de processamento";
            log.Info(logMessage);

            return await Result<bool>.SuccessAsync(logMessage);
        }


        public void TotalMessagesPending(string queueName)
        {
            var result = channel.QueueDeclarePassive(queueName);
            TotalMessages = Convert.ToInt16(result.MessageCount);
        }


        public Result<bool> StartConsumer()
        {
            // Retorna a quantidade de mensagens pendentes na fila
            TotalMessagesPending(Helper.Constants.RABBITMQ_QUEUENAME);

            var serverResponse = IsServerAccessible();
            if (!serverResponse.success)
                return Result<bool>.Fail(serverResponse.xmessage);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += OnConsumeReceived;
            channel.BasicConsume(queue: Helper.Constants.RABBITMQ_QUEUENAME, autoAck: false, consumer: consumer);

            log.Info("Iniciando consumo de mensagens.");
            // Espera a Fila Ser processada
            WaitForMessagesToProcess();

            return Result<bool>.Success("Mensagens processadas.");
        }

        private void WaitForMessagesToProcess()
        {
            while ((TotalMessages > 0) || (ProcessessingData == true))
                Thread.Sleep(1000);
        }

        private async void OnConsumeReceived(object? sender, BasicDeliverEventArgs e)
        {
            ProcessessingData = true;
            await this.queue.HandleMessageAsync(e, this.processor);
            TotalMessages--;
            ProcessessingData = false;
        }
    }
}
