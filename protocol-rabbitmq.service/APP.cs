using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using protocol.rabbitmq.shared.Interfaces;
using System.Globalization;

namespace protocol.rabbitmq.service
{
    public static class APP
    {
        public static WebApplication? StartAppConsole(this WebApplicationBuilder builder, string name = "RabbitQM")
        {
            return builder.StartApp(name, "1.0", 5001, true);
        }

        /// <summary>
        /// Configuracao Padrao
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static WebApplication? StartApp(
            this WebApplicationBuilder builder,
            string name = "RabbitQM",
            string version = "1.0",
            int PortHttp = 5001,
            bool console = false)
        {
            builder.Services.AddServices();
            // Change appsetings
            var appsettings = "appsettings.json";
            builder.Host

                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile(appsettings, optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                ;

            var app = builder.Build();

            // Configurar a cultura 
            var cultureInfo = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            //add url and port
            if (console == false)
            {
                var configuration = app.Services.GetRequiredService<IConfiguration>();
                app.Urls.Add($"http://0.0.0.0:{PortHttp}");
                app.UseCors("All");
                app.UseSwagger();
                app.UseSwaggerUI(x =>
                {
                    x.SwaggerEndpoint("/swagger/v1/swagger.json", $"{name} API {version}");
                });
                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();
                app.MapControllers();
            }

            var log = app.Services.GetRequiredService<IServiceLog>();
            try
            {
                if (console == false)
                {
                    log.Info($"Start {name} API versão {version}");
                    app.Run();
                }
                else
                    log.Info($"Start {name} Console versão {version}");

                return app;
            }
            catch (Exception e)
            {
                log.Error($"Erro ao iniciar o Microservico : {e.Message}");
                return null;
            }
        }
    }
}
