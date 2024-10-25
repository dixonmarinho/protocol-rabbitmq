using Microsoft.Extensions.DependencyInjection;
using protocol.rabbitmq.service.DependencyInject;

namespace protocol.rabbitmq.service
{
    // Inversion Of Control - Inversao de Controle
    public static partial class InversionOfControl
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddAutoMapperService()
                .AddServiceCustomCors()
                .AddServiceChangeCulture()
                .AddServicesGeneral()
                .AddServiceSwagger()
                .AddServiceLog()
                //.AddIdentityServices()
                .AddServiceData()
                // Ativando o MVC
                .AddServiceAuthentication()
                .AddServiceRabbitMQ()
                ;

            services.AddControllers();
            return services;
        }
    }
}
