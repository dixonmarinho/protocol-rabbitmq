using Microsoft.Extensions.DependencyInjection;

namespace protocol.rabbitmq.service.DependencyInject
{
    public static partial class DependencyInject
    {
        public static IServiceCollection AddServiceCustomCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("All",
                                        builder =>
                                        {
                                            builder
                                                //.SetIsOriginAllowed((host) => new Uri(host).Host == "localhost")
                                                .AllowAnyOrigin()
                                                .AllowAnyMethod()
                                                .AllowAnyHeader();
                                        });
            });
            return services;

        }

    }
}
