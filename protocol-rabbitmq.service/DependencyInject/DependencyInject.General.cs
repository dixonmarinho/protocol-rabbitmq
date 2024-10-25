using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace protocol.rabbitmq.service.DependencyInject
{
    public partial class DependencyInject
    {
        public static IServiceCollection AddServicesGeneral(this IServiceCollection services)
        {
            // Verifica se o arquivo appsettings.json existe, se não existir, cria um arquivo com as configurações padrão
            var appsettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            if (!File.Exists(appsettingsPath))
            {
                // Criar um dicionário com as configurações padrão
                var defaultSettings = new Dictionary<string, string>
                {
                    { "ConnectionStrings:Default", "" },
                    // adicione outras configurações padrão aqui
                };
                // Criar um arquivo appsettings.json com as configurações padrão
                var json = JsonConvert.SerializeObject(defaultSettings, Formatting.Indented);
                File.WriteAllText(appsettingsPath, json);
            }

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
