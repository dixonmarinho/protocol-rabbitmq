using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using protocol.rabbitmq.service.Services;
using protocol.rabbitmq.shared.Interfaces;
using Serilog;

namespace protocol.rabbitmq.service.DependencyInject
{
    public static partial class DependencyInject
    {
        private static string GetLevelName(Serilog.Events.LogEventLevel level)
        {
            var basedir = System.AppDomain.CurrentDomain.BaseDirectory;
            switch (level)
            {
                case Serilog.Events.LogEventLevel.Debug:
                    return "Debug";
                case Serilog.Events.LogEventLevel.Error:
                    return "Error";
                case Serilog.Events.LogEventLevel.Fatal:
                    return "Fatal";
                case Serilog.Events.LogEventLevel.Information:
                    return "Information";
                case Serilog.Events.LogEventLevel.Verbose:
                    return "Verbose";
                case Serilog.Events.LogEventLevel.Warning:
                    return "Warning";
                default:
                    return "Unknown";
            }

        }

        public static IServiceCollection AddServiceLog(this IServiceCollection services, bool StoredInDb = true)
        {
            var serviceProvider = services.BuildServiceProvider();
            var config = serviceProvider.GetService<IConfiguration>();
            var connectionstring = config.GetConnectionString("Default");


            // Configura o Logger - E faz um Nivelamento conforme o tipo
            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var dir = Path.GetDirectoryName(location);
            Directory.SetCurrentDirectory(dir!);
            var currentDirectory = Directory.GetCurrentDirectory();
            currentDirectory = Path.Combine(currentDirectory, "Logs");
            if (!Directory.Exists(currentDirectory))
                Directory.CreateDirectory(currentDirectory);

            Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.PostgreSQL(
                        connectionString: connectionstring,
                        needAutoCreateTable: true,
                        period: TimeSpan.FromSeconds(1),
                        tableName: "Logs")
                    .WriteTo.Console()
                    .WriteTo.Map(
                        evt => evt.Level,
                        (level, wt) =>
                        {
                            var levelName = GetLevelName(level);
                            wt.File($"{currentDirectory}/{levelName} .txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: level);
                        }
                    )
                    .CreateLogger()
                    ;
            // Cria o Servico
            services.AddSingleton<IServiceLog, ServiceLog>();
            return services;
        }
    }

}
