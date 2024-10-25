using Microsoft.Extensions.DependencyInjection;
using protocol.rabbitmq.shared.Models.Mapper;

namespace protocol.rabbitmq.service.DependencyInject
{
    public static partial class DependencyInject
    {
        public static IServiceCollection AddAutoMapperService(this IServiceCollection services)
        {
            // Configura o AutoMapper
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            var mapper = config.CreateMapper();
            // Adiciona o mapeamento ao provedor de serviços.
            services.AddSingleton(mapper);
            return services;

        }

    }
}
