using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace protocol.rabbitqm.test.Base
{
    public class Base_Service_Test<T> : Base_Test where T : class
    {
        private readonly WebApplicationFactory<T> _app;
        private readonly IServiceScope _scope;
        private readonly IServiceProvider _serviceProvider;
        private readonly HttpClient _cliente;

        public Base_Service_Test(ITestOutputHelper outputWriter) : base(outputWriter)
        {
            var path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory));
            _app = new WebApplicationFactory<T>()
                .WithWebHostBuilder(builder =>
                {
                    // Configure o builder do WebHost
                    builder.ConfigureAppConfiguration(config =>
                    {
                        config.SetBasePath(path);
#if DEBUG
                        config.AddJsonFile("appsettings.json");
#else
                        config.AddJsonFile("appsettings.json");
#endif
                    });
                    builder.ConfigureServices(services =>
                    {
                        // Registre os serviços aqui
                    });
                });

            // Crie o escopo e o ServiceProvider
            _scope = _app.Services.CreateScope();
            _serviceProvider = _scope.ServiceProvider;

            // Crie o HttpClient
            //_cliente = _app.CreateClient();
        }

        public T GetService<T>()
        {
            return _serviceProvider.GetRequiredService<T>();
        }

        // ... outros métodos ...
    }
}
