using Microsoft.Extensions.DependencyInjection;
using protocol.rabbitqm.service.DependencyInject;

namespace protocol.rabbitqm.service
{
    // Inversion Of Control - Inversao de Controle
    public static partial class InversionOfControl
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddServiceCustomCors()
                .AddServiceChangeCulture()
                .AddServicesGeneral()
                .AddServiceSwagger()
                .AddServiceLog()
                //.AddIdentityServices()
                .AddServiceData()
                .AddServiceProtocol()
                // Ativando o MVC
                .AddServiceAuthentication();

            services.AddControllers();
            return services;
        }
    }
}
