using Microsoft.Extensions.DependencyInjection;
using protocol.rabbitqm.service.Services;
using protocol.rabbitqm.shared.Interfaces;

namespace protocol.rabbitqm.service.DependencyInject
{
    public partial class DependencyInject
    {
        public static IServiceCollection AddServiceProtocol(this IServiceCollection services)
        {
            services
                .AddScoped<IServiceProtocol, ServiceProtocol>()
                ;
            return services;
        }

    }
}
