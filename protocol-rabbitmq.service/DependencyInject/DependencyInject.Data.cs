using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using protocol.rabbitmq.data.Data;
using protocol.rabbitmq.shared.Interfaces;

namespace protocol.rabbitmq.service.DependencyInject
{
    public static partial class DependencyInject
    {
        public static IServiceCollection AddServiceData(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var config = serviceProvider.GetService<IConfiguration>();
            var connectionstring = config.GetConnectionString("Default");

            services
                .AddDbContext<AppDataContext>(opt =>
                {
                    opt.UseNpgsql(connectionstring);
                })
                .AddScoped<IUnitOfWork<AppDataContext>, UnitOfWork<AppDataContext>>()
                ;
            return services;
        }
    }
}
