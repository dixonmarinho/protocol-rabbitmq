using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace protocol.rabbitqm.service.DependencyInject
{
    public partial class DependencyInject
    {
        public static IServiceCollection AddServicesGeneral(this IServiceCollection services)
        {
            // Registra o IConfiguration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange:
             true)
                .Build();
            services.AddSingleton<IConfiguration>(configuration);
            return services;
        }

    }
}
