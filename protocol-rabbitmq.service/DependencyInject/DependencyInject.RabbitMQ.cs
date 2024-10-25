using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using protocol.rabbitmq.service.Services;
using protocol.rabbitmq.service.Services.RabbitMQ;
using protocol.rabbitmq.shared.Helpers;
using protocol.rabbitmq.shared.Interfaces;
using RabbitMQ.Client;

namespace protocol.rabbitmq.service.DependencyInject
{
    public static partial class DependencyInject
    {

        public static IServiceCollection AddServiceRabbitMQ(this IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            var hostRabbitMQ = config.GetSection("RabbitMQ:Host").Value;
            var portRabbitMQ = config.GetSection("RabbitMQ:Port").Value;
            var userRabbitMQ = config.GetSection("RabbitMQ:Username").Value;
            var passwordRabbitMQ = config.GetSection("RabbitMQ:Password").Value;

            Helper.Constants.RabbitMQHost = hostRabbitMQ;
            Helper.Constants.RabbitMQPort = int.Parse(portRabbitMQ);


            services
                .AddScoped<IServiceProtocol, ServiceProtocol>()
                .AddSingleton(x => new ConnectionFactory
                {
                    HostName = Helper.Constants.RabbitMQHost,
                    Port = Helper.Constants.RabbitMQPort,
                    UserName = userRabbitMQ,
                    Password = passwordRabbitMQ
                });
            services.AddScoped<IModel>(sp =>
                {
                    var factory = sp.GetRequiredService<ConnectionFactory>();
                    var connection = factory.CreateConnection();
                    var channel = connection.CreateModel();
                    var args = new Dictionary<string, object> {
                            { "x-dead-letter-exchange", "my-dead-letter-exchange" },
                            { "x-dead-letter-routing-key", "my-dead-letter-routing-key" }
                    };
                    channel.ExchangeDeclare(exchange: Helper.Constants.RABBITMQ_EXCHANGENAME, type: ExchangeType.Direct);
                    channel.QueueDeclare(queue: Helper.Constants.RABBITMQ_QUEUENAME, durable: true, exclusive: false, autoDelete: false, arguments: args);
                    channel.QueueBind(queue: Helper.Constants.RABBITMQ_QUEUENAME, exchange: Helper.Constants.RABBITMQ_EXCHANGENAME, routingKey: Helper.Constants.RABBITMQ_ROUTINGKEY);
                    return channel;
                })

                .AddScoped<MessageProcessor>()
                .AddScoped<QueueManager>()
                .AddScoped<IServiceRabbitMQ, ServiceRabbitMQ>()
                ;
            return services;
        }
    }

}
