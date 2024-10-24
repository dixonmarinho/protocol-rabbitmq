using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using protocol.rabbitqm.shared.Interfaces;
using System.Globalization;

namespace protocol.rabbitqm.service
{
    public static class APP
    {
        /// <summary>
        /// Configuracao Padrao
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static void StartApp(this WebApplicationBuilder builder, string name = "tasks", string version = "1.0", int PortHttp = 5001)
        {
            builder.Services.AddServices();

            // Change appsetings
            var appsettings = "appsettings.json";
            builder.Host

                //.AbpMultiTenancyOptions
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile(appsettings, optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                //.UseAutofac()
                ;

            var app = builder.Build();

            // Configurar a cultura 
            var cultureInfo = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            //add url and port
            var configuration = app.Services.GetRequiredService<IConfiguration>();
            app.Urls.Add($"http://0.0.0.0:{PortHttp}");
            app.UseCors("All");
            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", $"{name} API {version}");
            });
            //}

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            var log = app.Services.GetRequiredService<IServiceLog>();
            log.Info($"Start {name} API versão {version}");
            try
            {
                app.Run();
            }
            catch (Exception e)
            {
                log.Error($"Erro ao iniciar o Microservico : {e.Message}");
            }
        }
    }
}
